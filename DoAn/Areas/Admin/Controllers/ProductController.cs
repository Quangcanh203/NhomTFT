using DoAn.Models;
using DoAn.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize]
    [Authorize(Roles = "Admin,Manage prodcut")]

    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _dataContext.Products
                .OrderByDescending(p => p.Id)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Category = new SelectList(_dataContext.category, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Category = new SelectList(_dataContext.category, "Id", "Name", product.CategoryId);
                ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
                return View(product);
            }

            product.slug = product.Name.Replace(" ", "-").ToLower();
            var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.slug == product.slug);
            if (existingProduct != null)
            {
                ModelState.AddModelError("", "Sản phẩm đã có trong dữ liệu");
                ViewBag.Category = new SelectList(_dataContext.category, "Id", "Name", product.CategoryId);
                ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
                return View(product);
            }

            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageUpload.CopyToAsync(fs);
                }
                product.Image = imageName;
            }

            _dataContext.Add(product);
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Thêm sản phẩm thành công";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var product = await _dataContext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Category = new SelectList(_dataContext.category, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModel product)
        {
            var existingProduct = await _dataContext.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            ViewBag.Category = new SelectList(_dataContext.category, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            if (ModelState.IsValid)
            {
                existingProduct.slug = product.Name.Replace(" ", "-").ToLower();

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpload.CopyToAsync(fs);
                    }

                    if (!string.IsNullOrEmpty(existingProduct.Image))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, existingProduct.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    existingProduct.Image = imageName;
                }

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.BrandId = product.BrandId;

                _dataContext.Update(existingProduct);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var product = await _dataContext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }

            if (!string.Equals(product.Image, "noname.jpg", StringComparison.OrdinalIgnoreCase))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string oldfileImage = Path.Combine(uploadsDir, product.Image);
                if (System.IO.File.Exists(oldfileImage))
                {
                    System.IO.File.Delete(oldfileImage);
                }
            }

            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Sản phẩm đã xóa";
            return RedirectToAction("Index");
        }
    }
}
