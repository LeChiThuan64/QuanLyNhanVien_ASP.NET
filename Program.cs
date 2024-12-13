using Microsoft.EntityFrameworkCore;
using PhongUserManagement.Models;
using YourProjectNamespace.Models;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext với Dependency Injection
builder.Services.AddDbContext<PhongUserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PhongUserDb")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
