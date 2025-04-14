using MelodiaBG.Data;
using MelodiaBG.Extensions;
using MelodiaBG.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MelodiaBG.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartCookie = "ShoppingCart";
        private readonly IConfiguration _configuration;


        public CartController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }

        public IActionResult AddToCart(int id)
        {
            var instrument = _context.Instruments.Find(id);
            if (instrument == null) return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.InstrumentId == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    InstrumentId = instrument.Id,
                    Name = instrument.Name,
                    Price = instrument.Price,
                    Quantity = 1,
                    ImageUrl = instrument.ImageUrl
                });
            }
            else
            {
                item.Quantity++;
            }

            Response.Cookies.SetObject(CartCookie, cart);
            return RedirectToAction("Index", "Product");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.InstrumentId == id);
            if (item != null)
            {
                cart.Remove(item);
            }
            Response.Cookies.SetObject(CartCookie, cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ProcessPayment()
        {
            var cart = GetCart();
            if (cart.Count == 0)
            {
                ViewBag.Error = "Количката е празна!";
                return View("Checkout");
            }

            return RedirectToAction("CreateCheckoutSession", "Stripe");
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.StripePublicKey = _configuration["Stripe:PublishableKey"];

            return View(cart);
        }

        [Authorize]
        public async Task<IActionResult> OrderSuccess()
        {
            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;


            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // or handle guest checkout
            }

            // 1. Create Order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(item => item.Price * item.Quantity),
                OrderDetails = new List<OrderDetail>()
            };

            // 2. Create OrderDetails
            foreach (var item in cart)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    InstrumentId = item.InstrumentId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }

            // 3. Save to DB
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 4. Clear the cart
            Response.Cookies.SetObject(CartCookie, new List<CartItem>());

            return View();
        }



        private List<CartItem> GetCart()
        {
            return Request.Cookies.GetObject<List<CartItem>>(CartCookie) ?? new List<CartItem>();
        }

        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            return Json(new { count = cart.Sum(i => i.Quantity) });
        }
    }
}
