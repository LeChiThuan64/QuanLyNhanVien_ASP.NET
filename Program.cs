using Microsoft.EntityFrameworkCore;
using PhongUserManagement.Models;
using YourProjectNamespace.Models;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext với Dependency Injection
builder.Services.AddDbContext<PhongUserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PhongUserDb")));

// Thêm dịch vụ Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Thời gian timeout của session
    options.Cookie.HttpOnly = true; // Cookie chỉ dùng cho HTTP (bảo mật)
    options.Cookie.IsEssential = true; // Cần thiết cho ứng dụng hoạt động
});

// Thêm MVC services vào container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sử dụng Session Middleware trước khi Authorization
app.UseSession();

app.UseAuthorization();

// Cấu hình route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=DangNhap}/{id?}");

app.Run();
