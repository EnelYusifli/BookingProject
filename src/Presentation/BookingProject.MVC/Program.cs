using BookingProject.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	   .AddCookie(options =>
	   {
		   options.Cookie.Name = ".AspNetCore.Identity.Application"; 
		   options.Cookie.SameSite = SameSiteMode.None;
		   options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
		   options.Cookie.HttpOnly = true;
		   options.ExpireTimeSpan = TimeSpan.FromDays(4);
	   });
//builder.Services.AddScoped<UserManager<AppUser>>();
builder.Services.AddSession(opt =>
{
    opt.Cookie.Name = "token";
});
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

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
