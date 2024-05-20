using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.ProfileViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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

			Response.Cookies.Append("token", token,
				new CookieOptions
				{
					Expires = DateTime.UtcNow.AddMinutes(4),
					HttpOnly = true,
					Secure = true,
					IsEssential = true,
					SameSite = SameSiteMode.None
				});

			var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, vm.UserName)

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
		ModelState.AddModelError("", "Invalid credentials");
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
			return RedirectToAction("ResetPasswordInfo");
		}
		return View();
	}
    public IActionResult ResetPasswordInfo()
    {
        return View();
    }
    public IActionResult ResetPassword(string token)
	{
		ResetPasswordViewModel vm = new() {Token=token};
        return View(vm);
	}
	[HttpPost]
	public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
	{
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync(baseAddress + $"/acc/resetpassword", stringContent);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index","Home");
		}
		return View();
	}
	public async Task<IActionResult> Profile()
	{
		//if (!ModelState.IsValid) return View();
		ProfileViewModel profileViewModel = new();
		profileViewModel.PersonalInfo = new();
		profileViewModel.Password = new();
		var response = await _httpClient.GetAsync(baseAddress + "/acc/getauthuser");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dto = JsonConvert.DeserializeObject<UserViewModel>(responseData);
			profileViewModel.User = dto;
			return View(profileViewModel);
		}
		return RedirectToAction("Index", "Home");
	}
	[HttpPost]
	public async Task<IActionResult> UpdatePersonalInfo(ProfileViewModel vm)
	{
        using (var content = new MultipartFormDataContent())
        {
            // Add individual properties as StringContent
            content.Add(new StringContent(vm.PersonalInfo.Id), nameof(vm.PersonalInfo.Id));
            content.Add(new StringContent(vm.PersonalInfo.FirstName), nameof(vm.PersonalInfo.FirstName));
            content.Add(new StringContent(vm.PersonalInfo.LastName), nameof(vm.PersonalInfo.LastName));
            content.Add(new StringContent(vm.PersonalInfo.Email), nameof(vm.PersonalInfo.Email));
            content.Add(new StringContent(vm.PersonalInfo.UserName), nameof(vm.PersonalInfo.UserName));

            if (vm.PersonalInfo.PhoneNumber != null)
            {
                content.Add(new StringContent(vm.PersonalInfo.PhoneNumber), nameof(vm.PersonalInfo.PhoneNumber));
            }

            if (vm.PersonalInfo.Birthdate.HasValue)
            {
                content.Add(new StringContent(vm.PersonalInfo.Birthdate.Value.ToString("yyyy-MM-dd")), nameof(vm.PersonalInfo.Birthdate));
            }

            // If there is a profile photo, add it to the multipart form data content
            if (vm.PersonalInfo.ProfilePhoto != null)
            {
                var fileContent = new StreamContent(vm.PersonalInfo.ProfilePhoto.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(vm.PersonalInfo.ProfilePhoto.ContentType);
                content.Add(fileContent, nameof(vm.PersonalInfo.ProfilePhoto), vm.PersonalInfo.ProfilePhoto.FileName);
            }

            var response = await _httpClient.PutAsync(baseAddress + "/acc/updateuser", content);

            // Check if the response indicates success
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Log the response content for debugging
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                Console.WriteLine(responseContent);
            }
        }

        // If the request failed, return the view with the original model to display validation errors
        return View(vm);
    }
	[HttpPost]
	public async Task<IActionResult> UpdatePassword(ProfileViewModel vm)
	{
		var dataStr = JsonConvert.SerializeObject(vm.Password);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync(baseAddress + "/acc/changepassword", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index", "Home");
		}
		return RedirectToAction("Profile");
	}
}

