using homework.Data;
using homework.Models;
using homework.Services;
using homework.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using homework.Services.email;

namespace homework.Controllers
{
    public class ProductController : Controller
    {
        private const string ProductImageFolder = "Images";

        private readonly ApplicationDbContext _db;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            ApplicationDbContext db,
            IFileService fileService,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<ProductController> logger)
        {
            _db = db;
            _fileService = fileService;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? searchKey)
        {
            var products = _db.Products
                .Include(product => product.Category)
                .Where(product => !product.IsDelete);

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                products = products.Where(product =>
                    product.Name.Contains(searchKey) ||
                    product.Description.Contains(searchKey) ||
                    product.Category.Name.Contains(searchKey));
            }

            ViewData["SearchKey"] = searchKey;

            return View(await products.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel input)
        {
            if (input.ImageFile == null || input.ImageFile.Length == 0)
            {
                ModelState.AddModelError(
                    nameof(input.ImageFile),
                    "Please choose a product image.");
            }
            else if (!IsAllowedImage(input.ImageFile))
            {
                ModelState.AddModelError(
                    nameof(input.ImageFile),
                    "Choose a JPG, PNG, or WEBP image up to 5 MB.");
            }

            if (!ModelState.IsValid)
                return View(input);

            var imageFileName = await _fileService.SaveFile(
                input.ImageFile,
                ProductImageFolder);

            if (imageFileName == null)
            {
                ModelState.AddModelError(
                    nameof(input.ImageFile),
                    "The image could not be saved.");

                return View(input);
            }

            var currentUser = User.Identity?.Name ?? "System";

            var product = new Product
            {
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                CategoryId = input.CategoryId,
                ImageUrl = $"/{ProductImageFolder}/{imageFileName}",
                CreatedBy = currentUser,
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = currentUser,
                UpdatedAt = DateTime.UtcNow,
                IsDelete = false
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            var recipient = _configuration["Email:NotificationTo"];

            if (!string.IsNullOrWhiteSpace(recipient))
            {
                try
                {
                    await _emailService.Send(
                        recipient,
                        "New Product!",
                        $"New Product With name: {product.Name}");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Product was saved, but the email notification could not be sent.");
                }
            }

            TempData["ProductSuccess"] = "Product created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _db.Products
                .FirstOrDefaultAsync(item =>
                    item.Id == id && !item.IsDelete);

            if (product == null)
                return NotFound();

            var input = new UpdateProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CurrentImageUrl = product.ImageUrl
            };

            return View(input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            int id,
            UpdateProductViewModel input)
        {
            if (id != input.Id)
                return NotFound();

            if (input.ImageFile != null &&
                input.ImageFile.Length > 0 &&
                !IsAllowedImage(input.ImageFile))
            {
                ModelState.AddModelError(
                    nameof(input.ImageFile),
                    "Choose a JPG, PNG, or WEBP image up to 5 MB.");
            }

            if (!ModelState.IsValid)
                return View(input);

            var product = await _db.Products
                .FirstOrDefaultAsync(item =>
                    item.Id == id && !item.IsDelete);

            if (product == null)
                return NotFound();

            product.Name = input.Name;
            product.Description = input.Description;
            product.Price = input.Price;
            product.CategoryId = input.CategoryId;
            product.UpdatedBy = User.Identity?.Name ?? "System";
            product.UpdatedAt = DateTime.UtcNow;

            string? previousImage = null;

            if (input.ImageFile != null && input.ImageFile.Length > 0)
            {
                previousImage = product.ImageUrl;

                var imageFileName = await _fileService.SaveFile(
                    input.ImageFile,
                    ProductImageFolder);

                if (imageFileName == null)
                {
                    ModelState.AddModelError(
                        nameof(input.ImageFile),
                        "The image could not be saved.");

                    input.CurrentImageUrl = product.ImageUrl;

                    return View(input);
                }

                product.ImageUrl =
                    $"/{ProductImageFolder}/{imageFileName}";
            }

            await _db.SaveChangesAsync();

            if (previousImage != null)
            {
                await _fileService.DeleteFile(
                    ProductImageFolder,
                    Path.GetFileName(previousImage));
            }

            TempData["ProductSuccess"] = "Product updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products
                .FirstOrDefaultAsync(item =>
                    item.Id == id && !item.IsDelete);

            if (product == null)
                return NotFound();

            var imageFileName = Path.GetFileName(product.ImageUrl);

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            await _fileService.DeleteFile(
                ProductImageFolder,
                imageFileName);

            TempData["ProductSuccess"] = "Product deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private static bool IsAllowedImage(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            return file.Length <= 5 * 1024 * 1024 &&
                   new[] { ".jpg", ".jpeg", ".png", ".webp" }
                       .Contains(extension);
        }
    }
}