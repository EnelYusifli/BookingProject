using BookingProject.Application.Services.Implementations;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;
using Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromDays(1);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
//builder.Services.AddAuthentication(opt =>
//{
//	opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//	opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//	opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//}).AddCookie(options =>
//{
//	options.Cookie.Name = ".AspNetCore.Identity.Application";
//	// Additional cookie options/configuration if needed
//});

//builder.Services.AddScoped<UserManager<AppUser>>();
//builder.Services.AddSession(opt =>
//{
//    opt.Cookie.Name = "token";
//});
//builder.Services.AddHttpClient("ApiClient")
//		.AddHttpMessageHandler<CustomHttpClientHandler>();
//builder.Services.AddScoped<ITokenService, TokenService>();

//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//	options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
//});
//builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<TokenRefreshMiddleware>();
//app.UseMiddleware<SaveApiCookiesMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
