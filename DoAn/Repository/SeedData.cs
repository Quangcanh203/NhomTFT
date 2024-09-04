using DoAn.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();

			if (!_context.Products.Any())
			{
				CategoryModel macbook = new CategoryModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is large in the word", Status = 1 };
				CategoryModel pc = new CategoryModel { Name = "Pc", Slug = "pc", Description = "Pc is large in the word", Status = 1 };
				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is large in the word", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "samsung", Description = "Samsung is large in the word", Status = 1 };

				// Thêm các Category và Brand vào context
				_context.category.AddRange(macbook, pc);
				_context.Brands.AddRange(apple, samsung);

				_context.Products.AddRange(
					new ProductModel { Name = "Macbook", slug = "macbook", Description = "Macbook is Best", Image = "1.jpg", Category = macbook, Brand = apple, Price = 123 },
					new ProductModel { Name = "Pc", slug = "pc", Description = "Pc is Best", Image = "2.jpg", Category = pc, Brand = samsung, Price = 123 }
				);

				_context.SaveChanges();
			}
		}

	}
}
