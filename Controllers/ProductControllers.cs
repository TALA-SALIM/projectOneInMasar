using homework.Data;
using homework.Models;
using homework.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace homework.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var products = _db.Products
                .Include(x => x.Category)
                .Where(x => !x.IsDelete)
                .ToList();

            return View(products);
        }

    
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

  [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(CreateProductViewModel input)
{
    if (!ModelState.IsValid)
    {
        return View(input);
    }

    var currentUser = User.Identity?.Name ?? "System";

    var product = new Product
    {
        Name = input.Name,
        Description = input.Description,
        Price = input.Price,
        CategoryId = input.CategoryId,
        ImageUrl = input.ImageUrl,

        CreatedBy = currentUser,
        CreatedAt = DateTime.UtcNow,

        UpdatedBy = currentUser,
        UpdatedAt = DateTime.UtcNow,

        IsDelete = false
    };

    _db.Products.Add(product);
    _db.SaveChanges();

    return RedirectToAction(nameof(Index));
}
    }
}