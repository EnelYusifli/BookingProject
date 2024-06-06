using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Implementations;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using BookingProject.MVC.Services;
using BookingProject.Persistence.Contexts;
using BookingProject.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddSession();
builder.Services.AddScoped<ILoginService,LoginService>();
builder.Services.AddScoped<IRoomRepository,RoomRepository>();

builder.Services.AddAuthentication(opt =>
{
	opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
	opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
	options.Cookie.Name = ".AspNetCore.Identity.Application";
}).AddGoogle(GoogleDefaults.AuthenticationScheme,googleOptions =>
{
	googleOptions.ClientId = builder.Configuration["Authentication:GoogleClientId"];
	googleOptions.ClientSecret = builder.Configuration["Authentication:GoogleClientSecret"];
	googleOptions.CallbackPath = "/signin-google";
});

//builder.Services.AddScoped<UserManager<AppUser>>();
//builder.Services.AddSession(opt =>
//{
//    opt.Cookie.Name = "token";
//});
//builder.Services.AddHttpClient("ApiClient")
//		.AddHttpMessageHandler<CustomHttpClientHandler>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseMiddleware<TokenRefreshMiddleware>();
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
