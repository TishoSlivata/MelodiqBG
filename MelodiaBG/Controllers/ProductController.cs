using MelodiaBG.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MelodiaBG.Models;
using MelodiaBG.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;


public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Показване на всички продукти
    public async Task<IActionResult> Index(string searchString)
    {
        var instruments = from i in _context.Instruments
                          select i;

        if (!string.IsNullOrEmpty(searchString))
        {
            instruments = instruments.Where(i => i.Name.Contains(searchString) || i.Description.Contains(searchString));
        }

        return View(await instruments.ToListAsync());
    }

    // Детайли за продукт
    public IActionResult Details(int id)
    {
        var instrument = _context.Instruments
            .Include(i => i.Category)
            .Include(i => i.Reviews)
            .FirstOrDefault(i => i.Id == id);

        if (instrument == null) return NotFound();

        return View(instrument);
    }



    // Форма за създаване
    [Authorize(Roles ="Admin")]
    public IActionResult Create()
    {
        ViewData["Categories"] = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InstrumentCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = _context.Categories.ToList();
            return View(viewModel);
        }

        var instrument = new Instrument
        {
            Name = viewModel.Name,
            Description = viewModel.Description,
            Price = viewModel.Price,
            CategoryId = viewModel.CategoryId
        };

        if (viewModel.Image != null)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/instruments");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await viewModel.Image.CopyToAsync(stream);
            }

            instrument.ImageUrl = "/images/instruments/" + fileName;
        }
        else
        {
            instrument.ImageUrl = "/images/default.png"; // Записвай резервно изображение
        }


        // Запис в базата
        _context.Add(instrument);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var instrument = await _context.Instruments.FirstOrDefaultAsync(i => i.Id == id);

        if (instrument == null)
        {
            return NotFound();
        }

        var model = new EditInstrumentViewModel
        {
            InstrumentId = instrument.Id,
            Name = instrument.Name,
            Description = instrument.Description,
            Price = instrument.Price,
            CategoryId = instrument.CategoryId,
            ImageUrl = instrument.ImageUrl
        };

        ViewData["Categories"] = _context.Categories.ToList();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditInstrumentViewModel model)
    {
        if (id != model.InstrumentId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = _context.Categories.ToList();
            return View(model);
        }

        var instrument = await _context.Instruments.FindAsync(id);
        if (instrument == null)
        {
            return NotFound();
        }

        // Update instrument fields
        instrument.Name = model.Name;
        instrument.Description = model.Description;
        instrument.Price = model.Price;
        instrument.CategoryId = model.CategoryId;

        if (model.Image != null)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/instruments");

            // Remove old image if it's not the default one
            if (!string.IsNullOrEmpty(instrument.ImageUrl) && instrument.ImageUrl != "/images/default.png")
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", instrument.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Save new image
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            // Update database with new image URL
            instrument.ImageUrl = "/images/instruments/" + fileName;
        }

        try
        {
            _context.Update(instrument);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Instruments.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }
    
    // Форма за изтриване
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var instrument = await _context.Instruments.FirstOrDefaultAsync(m => m.Id == id);
        if (instrument == null) return NotFound();
        return View(instrument);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var instrument = await _context.Instruments.FindAsync(id);
        if (instrument == null) return NotFound();

        // Check if there is an image URL and if it's not the default image
        if (instrument.ImageUrl != "/images/default.png")
        {
            // Build the full path to the image file
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", instrument.ImageUrl.TrimStart('/'));

            // Check if the file exists, then delete it
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        // Remove the instrument from the database
        _context.Instruments.Remove(instrument);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}

