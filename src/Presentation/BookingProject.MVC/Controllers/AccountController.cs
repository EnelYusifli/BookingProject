using BookingProject.MVC.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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
		if (!ModelState.IsValid) return View();
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/acc/login", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index","Home");
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
