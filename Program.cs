using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MobileShopInMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Razorpay.Api;
using MobileShopInMVC.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Stripe Configuration
Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Authentication and Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // Redirect to login page if unauthorized
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add Session Support
builder.Services.AddSession(options =>
{
    // Optionally configure session properties (e.g., timeout) if needed
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Example session timeout
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Use Session middleware (only once)
app.UseSession();

app.UseRouting();

// Use Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map default route for controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
