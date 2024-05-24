using BookingProject.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(opt =>
{
	opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
	options.Cookie.Name = ".AspNetCore.Identity.Application";
	// Additional cookie options/configuration if needed
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
 //app.UseMiddleware<SaveApiCookiesMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
