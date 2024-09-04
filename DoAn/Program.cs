using DoAn.Areas.Admin.Repository;
using DoAn.Models;
using DoAn.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kết nối đến cơ sở dữ liệu
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectionDb"]);
});

// Thêm dịch vụ email
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Thêm các dịch vụ vào container
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

builder.Services.AddIdentity<AddUserModel, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>() // Sử dụng DataContext cho Identity
    .AddDefaultTokenProviders();

// Cấu hình các tùy chọn cho Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    // Cấu hình mật khẩu
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true; // Ký tự thường
    options.Password.RequireNonAlphanumeric = false; // Không yêu cầu ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không yêu cầu ký tự in hoa
    options.Password.RequiredLength = 4; // Độ dài tối thiểu

    // Cấu hình yêu cầu email duy nhất
    options.User.RequireUniqueEmail = true;
});

// Cấu hình cookie cho xác thực
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cấu hình cookie
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Account/Login"; // Sử dụng cấu hình LoginPath từ code mới
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Cấu hình AccessDeniedPath
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Cấu hình xử lý lỗi
app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");

app.UseSession();

// Cấu hình middleware cho ứng dụng
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Cấu hình tuyến đường cho khu vực Admin
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Cấu hình tuyến đường cho Category
app.MapControllerRoute(
    name: "category",
    pattern: "/category/{Slug?}",
    defaults: new { controller = "Category", action = "Index" });

// Cấu hình tuyến đường cho Brand
app.MapControllerRoute(
    name: "brand",
    pattern: "/brand/{Slug?}",
    defaults: new { controller = "Brand", action = "Index" });

// Cấu hình tuyến đường cho Order
app.MapControllerRoute(
    name: "order",
    pattern: "/order/{action=Index}/{id?}",
    defaults: new { controller = "Order", action = "Index" });

// Cấu hình tuyến đường mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Khởi tạo dữ liệu mẫu khi ứng dụng khởi động
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
