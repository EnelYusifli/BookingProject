using BookingProject.MVC.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNet.Identity;

namespace BookingProject.MVC.Controllers;

public class AccountController : Controller
{
	Uri baseAddress = new Uri("https://localhost:7197/api");
	private readonly HttpClient _httpClient;

	public AccountController(HttpClient httpClient)
	{
		_httpClient = httpClient;
		_httpClient.BaseAddress = baseAddress;
	}
	public IActionResult Login()
    {
        return View();
    }
	[HttpPost]
	public async Task<IActionResult> Login(LoginViewModel vm)
	{
		if (!ModelState.IsValid)
			return View();

		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/acc/login", stringContent);

		if (response.IsSuccessStatusCode)
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			var tokenObject = JObject.Parse(responseContent);
			var token = tokenObject["token"].ToString();

			// Save the token in cookie
			Response.Cookies.Append("token", token,
				new CookieOptions
				{
					Expires = DateTime.UtcNow.AddDays(4),
					HttpOnly = true,
					Secure = true,
					IsEssential = true,
					SameSite = SameSiteMode.None
				});

			// Create and sign in the user
			var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, vm.UserName)

            // Add more claims as needed
        };

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var authProperties = new AuthenticationProperties
			{
				ExpiresUtc = DateTimeOffset.UtcNow.AddDays(4),
				IsPersistent = true,
				AllowRefresh = true,
			};

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

			return RedirectToAction("Index", "Home");
		}
		return View();
	}

	public IActionResult Register()
    {
        return View();
    }
	[HttpPost]
	public async Task<IActionResult> Register(RegisterViewModel vm)
	{
		if (!ModelState.IsValid) return View();
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/acc/register", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Login");
		}
		return View();
	}
	public IActionResult ForgotPassword()
    {
        return View();
    }
	[HttpPost]
	public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
	{
		if (!ModelState.IsValid) return View();
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/acc/forgotpassword", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("TwoFactorAuth");
		}
		return View();
	}
	public IActionResult TwoFactorAuth()
	{
		return View();
	}
}
