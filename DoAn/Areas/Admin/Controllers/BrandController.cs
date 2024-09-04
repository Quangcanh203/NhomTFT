using DoAn.Models;
using DoAn.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    ///xác thực thì ms đc vào
	[Authorize(Roles ="Admin,Manage prodcut")]

	public class BrandController : Controller
    {
        private readonly DataContext _dataContext;

        public BrandController(DataContext context)
        {
            _dataContext = context;
        }
        [Route("Index")]

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands
                .OrderByDescending(c => c.Id)
                .ToListAsync());
        }
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-").ToLower();
                var existingCategory = await _dataContext.category.FirstOrDefaultAsync(c => c.Slug == brand.Slug);

                if (existingCategory != null)
                {
                    ModelState.AddModelError("", "Danh mục đã tồn tại.");
                    return View(brand);
                }

                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Thêm danh mục thành công.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Có một vài lỗi cần khắc phục.";
                return View(brand);
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _dataContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-").ToLower();

                var existingBrand = await _dataContext.Brands.FirstOrDefaultAsync(c => c.Slug == brand.Slug && c.Id != brand.Id);
                if (existingBrand != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã tồn tại.");
                    return View(brand);
                }

                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Cập nhật thương hiệu thành công.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Có một vài lỗi cần khắc phục.";
                return View(brand);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _dataContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Xóa thương hiệu thành công.";
            return RedirectToAction("Index");
        }
    }
}
