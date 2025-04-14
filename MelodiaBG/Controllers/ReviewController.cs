using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MelodiaBG.Models;
using MelodiaBG.Data;
using Microsoft.EntityFrameworkCore;

public class ReviewController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReviewController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Create(Review review)
    {
        if (ModelState.IsValid)
        {
            review.UserId = User.GetUserId();
            review.CreatedDate = DateTime.Now;

            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        return RedirectToAction("Details", "Product", new { id = review.InstrumentId });
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var review = _context.Reviews.Find(id);
        if (review != null)
        {
            var instrumentId = review.InstrumentId;

            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return RedirectToAction("Details", "Product", new { id = instrumentId });
        }

        return NotFound();
    }
}
