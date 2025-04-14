using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using MelodiaBG.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using MelodiaBG.Extensions;

namespace ExplorersDream.Controllers
{
    public class StripeController : Controller
    {
        private readonly IConfiguration _configuration;

        public StripeController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost]
        public IActionResult CreateCheckoutSession()
        {
            var cart = Request.Cookies.GetObject<List<CartItem>>("ShoppingCart");

            var cartItems = GetCart();

            if (cartItems == null || cartItems.Count == 0)
            {
                return Json(new { error = "Количката е празна." });
            }

            var domain = $"{Request.Scheme}://{Request.Host}";

            var lineItems = cartItems.Select(cartItem => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "bgn",
                    UnitAmount = (long)(cartItem.Price * 100), // Convert to cents
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = cartItem.Name
                    }
                },
                Quantity = cartItem.Quantity
            }).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = $"{domain}/Stripe/Success",
                CancelUrl = $"{domain}/Stripe/Cancel",
                Locale = "bg"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { sessionId = session.Id });
        }

        public IActionResult Success()
        {
            return RedirectToAction("OrderSuccess", "Cart");
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Cart");
        }

        private List<CartItem> GetCart()
        {
            var cart = Request.Cookies.GetObject<List<CartItem>>("ShoppingCart") ?? new List<CartItem>();
            return cart;
        }
        public IActionResult Checkout()
        {
            ViewBag.StripePublicKey = _configuration["Stripe:PublishableKey"];
            return View();
        }
    }
}
