using DoAn.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Repository
{
	public class DataContext : IdentityDbContext<AddUserModel>	
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) 
		{
		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> category { get; set; }
		public DbSet<OtherModel> Others { get; set; }
		public DbSet<OtherDetail> OtherDetails { get; set; }

    }
}
