﻿using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.MVC.Models;
using BookingProject.MVC.Services;
using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.HomeViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.ProfileViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace BookingProject.MVC.Controllers;

public class AccountController : Controller
{
	Uri baseAddress = new Uri("https://localhost:7197/api");
	
	private readonly HttpClient _httpClient;
	private readonly IRoomRepository _roomRepository;
	private readonly UserManager<AppUser> _userManager;
    private readonly ILoginService _loginService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(HttpClient httpClient,IRoomRepository roomRepository,UserManager<AppUser> userManager,ILoginService loginService,IHttpContextAccessor httpContextAccessor)
	{
		_httpClient = httpClient;
		_roomRepository = roomRepository;
		_userManager = userManager;
        _loginService = loginService;
        _httpContextAccessor = httpContextAccessor;
        _httpClient.BaseAddress = baseAddress;
	}
    private async Task<AppUser> GetCurrentUserAsync()
    {
        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
        }
        return null;
    }
	public IActionResult LoginWithGoogle()
	{
		var redirectUrl = Url.Action(nameof(GoogleResponse), "Account");
		var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
		return Challenge(properties, GoogleDefaults.AuthenticationScheme);
	}

	public async Task<IActionResult> GoogleResponse()
	{
		await _loginService.LoginWithGoogle();

		return RedirectToAction("Index", "Home");
	}
	public IActionResult RegisterWithGoogle()
	{
		var redirectUrl = Url.Action(nameof(GoogleRegisterResponse), "Account");
		var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
		return Challenge(properties, GoogleDefaults.AuthenticationScheme);
	}

	public async Task<IActionResult> GoogleRegisterResponse()
	{
		await _loginService.RegisterWithGoogle();

		return RedirectToAction("Account", "Login");
	}


	public IActionResult Login(string? returnUrl)
	{
		if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
		{
			HttpContext.Session.SetString("ReturnUrl", returnUrl);
		}
		return View();
	}
	[Authorize(Roles = "Customer,Owner")]
	public async Task<IActionResult> DeleteUser()
	{
		var user = await GetCurrentUserAsync();

		if (user == null)
		{
			return RedirectToAction("Index", "Home");
		}

		var result = await _userManager.DeleteAsync(user);

		if (result.Succeeded)
		{
			await Logout();

			return RedirectToAction("Index", "Home");
		}
		else
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return RedirectToAction("profile");
		}
	}
	[HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View();
		try
		{
			await _loginService.LoginUser(vm);
			if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}
		catch (Exception ex)
		{
			TempData["SuccessMessage"] = "Invalid credentials";

			Console.WriteLine(ex.Message);
			return View();
		}
        //ModelState.AddModelError("", "Invalid credentials");
        //return View();
        //var dataStr = JsonConvert.SerializeObject(vm);
        //var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        //var response = await _httpClient.PostAsync(baseAddress + "/acc/login", stringContent);

        //if (response.IsSuccessStatusCode)
        //{
        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    var tokenObject = JObject.Parse(responseContent);
        //    var token = tokenObject["token"].ToString();
        //    var refreshToken = tokenObject["refreshToken"].ToString();

        //    Response.Cookies.Append("token", token, new CookieOptions
        //    {
        //        Expires = DateTime.UtcNow.AddMinutes(10),
        //        HttpOnly = true,
        //        Secure = true,
        //        IsEssential = true,
        //        SameSite = SameSiteMode.None
        //    });

        //    Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        //    {
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        HttpOnly = true,
        //        Secure = true,
        //        IsEssential = true,
        //        SameSite = SameSiteMode.None
        //    });

        //    var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.Name, vm.UserName),
        //    new Claim("Token", token) // Add token as a custom claim if needed
        //};

        //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    var authProperties = new AuthenticationProperties
        //    {
        //        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(4),
        //        IsPersistent = true,
        //        AllowRefresh = true,
        //    };

        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            //return RedirectToAction("Index", "Home");
        //}

    }
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

		Response.Cookies.Delete("token");
		Response.Cookies.Delete("refreshToken");

		return RedirectToAction("Index", "Home");
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
		else if(response.StatusCode == HttpStatusCode.Conflict)
			ModelState.AddModelError("username", "Username already exists.");
		else if(response.StatusCode == HttpStatusCode.Gone)
			ModelState.AddModelError("email", "Email already exists.");
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
    [Authorize(Roles = "Customer,Owner,Admin")]
    public async Task<IActionResult> Profile()
	{
		if (!ModelState.IsValid) return View();
		ProfileViewModel profileViewModel = new();
		profileViewModel.PersonalInfo = new();
		profileViewModel.Password = new();
		AppUser user=await GetCurrentUserAsync();
        var response = await _httpClient.GetAsync(baseAddress + $"/users/getbyid/{user.Id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dto = JsonConvert.DeserializeObject<UserViewModel>(responseData);
			//AppUser appUser = new();
			//if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			//{
			//    appUser = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
			profileViewModel.User = dto;
			return View(profileViewModel);
		}
		return RedirectToAction("Index", "Home");
	}
    [Authorize(Roles = "Customer,Owner,Admin")]
    [HttpPost]
	public async Task<IActionResult> UpdatePersonalInfo(ProfileViewModel vm)
	{
        using (var content = new MultipartFormDataContent())
        {
            AppUser user = await GetCurrentUserAsync();
			vm.PersonalInfo.Id = user.Id;
			vm.User = null;
			vm.Password = null;
			if (!ModelState.IsValid)
			{
				var profileResponse = await _httpClient.GetAsync(baseAddress + $"/users/getbyid/{user.Id}");

				if (profileResponse.IsSuccessStatusCode)
				{
					var responseData = await profileResponse.Content.ReadAsStringAsync();
					var dto = JsonConvert.DeserializeObject<UserViewModel>(responseData);
					vm.User = dto;
					return View("Profile", vm);
				}

			}
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
                return RedirectToAction("Profile");
            }
            else
            {
                // Log the response content for debugging
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                Console.WriteLine(responseContent);
            }
        }
        return View("Profile",vm);
    }
    [Authorize(Roles = "Customer,Owner,Admin")]
    [HttpPost]
	public async Task<IActionResult> UpdatePassword(ProfileViewModel vm)
	{
			AppUser user = await GetCurrentUserAsync();
		vm.PersonalInfo = null;
		vm.User = null;
		if (!ModelState.IsValid)
		{
			var profileResponse = await _httpClient.GetAsync(baseAddress + $"/users/getbyid/{user.Id}");

			if (profileResponse.IsSuccessStatusCode)
			{
				var responseData = await profileResponse.Content.ReadAsStringAsync();
				var dto = JsonConvert.DeserializeObject<UserViewModel>(responseData);
				vm.User = dto;
				return View("Profile", vm);
			}

		}
		vm.Password.Id = user.Id;
		var dataStr = JsonConvert.SerializeObject(vm.Password);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync(baseAddress + "/acc/changepassword", stringContent);


		if (response.IsSuccessStatusCode)
		{
			TempData["SuccessMessage"] = "Password changed successfully.";
			var profileResponse = await _httpClient.GetAsync(baseAddress + $"/users/getbyid/{user.Id}");

			if (profileResponse.IsSuccessStatusCode)
			{
				var responseData = await profileResponse.Content.ReadAsStringAsync();
				var dto = JsonConvert.DeserializeObject<UserViewModel>(responseData);
				vm.User = dto;
				return View("Profile", vm);
			}
		}
		TempData["SuccessMessage"] = "Password  cannot be changed. Old password is not valid";
		var responseContent = await response.Content.ReadAsStringAsync();
		Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
		Console.WriteLine(responseContent);
		return RedirectToAction("Profile");
	}
    [Authorize(Roles = "Customer,Owner,Admin")]
    public async Task<IActionResult> Wishlist(int itemPerPage=5,int page=1)
	{
		List<WishlistViewModel> vms = new List<WishlistViewModel>();
		if (!ModelState.IsValid) return View();
		AppUser user = await GetCurrentUserAsync();
        var response = await _httpClient.GetAsync(baseAddress + $"/users/WishlistGetAll/{user.Id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			vms = JsonConvert.DeserializeObject<List<WishlistViewModel>>(responseData);
			var queryableItems = vms.Where(x=>!x.Hotel.IsDeactive).AsQueryable();
			var paginatedDatas = PaginatedList<WishlistViewModel>.Create(queryableItems, itemPerPage, page);
			return View(paginatedDatas);
		}
		return RedirectToAction("Index");
	}
	public async Task<IActionResult> RemoveAllFromWishlist()
	{
		List<WishlistViewModel> vms = new List<WishlistViewModel>();
		if (!ModelState.IsValid) return View();
		AppUser user = await GetCurrentUserAsync();
		var response = await _httpClient.GetAsync(baseAddress + $"/users/WishlistGetAll/{user.Id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			vms = JsonConvert.DeserializeObject<List<WishlistViewModel>>(responseData);
			foreach (var item in vms)
			{
				var response2 = await _httpClient.DeleteAsync(baseAddress + $"/users/removefromwishlist/{item.Id}");

				if (!response.IsSuccessStatusCode)
					return View("NotFound");
			}
			return RedirectToAction("Wishlist");
		}
		return View();
	}
    [Authorize(Roles = "Customer,Owner,Admin")]
    public async Task<IActionResult> AddtoWishlist(AddToWishlistViewModel vm)
	{
		AppUser user = await GetCurrentUserAsync();
		vm.Id = user.Id;
		//if (!ModelState.IsValid) return View();
        var dataStr = JsonConvert.SerializeObject(vm);
        var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(baseAddress + $"/users/addtowishlist", stringContent);

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotel = JsonConvert.DeserializeObject<HotelGetViewModel>(responseData);
			return RedirectToAction("wishlist");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return RedirectToAction("index","home");
	}
    [Authorize(Roles = "Customer,Owner,Admin")]
    public async Task<IActionResult> RemoveFromWishlist(int id)
	{
		if (!ModelState.IsValid) return View();
		var response = await _httpClient.DeleteAsync(baseAddress + $"/users/removefromwishlist/{id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotel = JsonConvert.DeserializeObject<HotelGetViewModel>(responseData);
			return RedirectToAction("wishlist");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return RedirectToAction("index", "home");
	}
    [Authorize(Roles = "Customer,Owner,Admin")]
    public async Task<IActionResult> Reservations(UserReservationsViewModel vm)
	{
		AppUser user = await GetCurrentUserAsync();
		var response = await _httpClient.GetAsync(baseAddress + $"/reservations/getallbyuser/{user.Id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dtos = JsonConvert.DeserializeObject<List<ReservationGetViewModel>>(responseData);
			vm.Review = null;
			vm.CancelledReservations=dtos.Where(x=>x.IsCancelled==true).ToList();
			vm.UpcomingReservations=dtos.Where(x=>x.StartTime>DateTime.Now && x.IsCancelled==false && x.IsDeactive==false).ToList();
			vm.CompletedReservations=dtos.Where(x=>x.StartTime<DateTime.Now && x.IsCancelled == false && x.IsDeactive == false) .ToList();
			return View(vm);
		}
		return RedirectToAction("Index", "Home");
	}
    [Authorize(Roles = "Customer,Owner,Admin")]
    public async Task<IActionResult> CancelReservation(int reservationId)
    {
        var response = await _httpClient.PutAsync(baseAddress + $"/reservations/cancel/{reservationId}", null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Reservations");
        }
        return RedirectToAction("Index","Home");
    }
    [Authorize(Roles = "Customer,Owner,Admin")]
    public IActionResult LeaveReview(int hotelid)
	{
		ViewBag.Id=hotelid;
		return View();
	}
    [Authorize(Roles = "Customer,Owner,Admin")]
    [HttpPost]
	public async Task<IActionResult> LeaveReview(ReviewCreateViewModel vm)
	{
		AppUser user = await GetCurrentUserAsync();
		vm.UserId = user.Id;
		//if (!ModelState.IsValid) {
		//	var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
		//	foreach (var error in errors)
		//	{
		//		Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
		//		Console.WriteLine(error.ErrorMessage);
		//	}
		//	return RedirectToAction("Index", "home"); }
		using (var content = new MultipartFormDataContent())
		{
			content.Add(new StringContent(vm.HotelId.ToString()), nameof(vm.HotelId));
			content.Add(new StringContent(vm.ReviewMessage), nameof(vm.ReviewMessage));
			content.Add(new StringContent(vm.UserId), nameof(vm.UserId));
			content.Add(new StringContent(vm.StarPoint.ToString()), nameof(vm.StarPoint));
		if (vm.ReviewImageFiles != null)
		{
			foreach (var file in vm.ReviewImageFiles)
			{
				if (file != null)
				{
					var fileContent = new StreamContent(file.OpenReadStream());
					fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
					content.Add(fileContent, nameof(vm.ReviewImageFiles), file.FileName);
				}
			}
		}
		var response = await _httpClient.PostAsync(baseAddress + "/reviews/create", content);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Reservations");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
			return RedirectToAction("Index","Home");

		}
		//var dataStr = JsonConvert.SerializeObject(vm);
		//var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		//var response = await _httpClient.PostAsync(baseAddress + "/reviews/create", stringContent);

		//if (response.IsSuccessStatusCode)
		//{
		//	return RedirectToAction("reservations");
		//}
		//return RedirectToAction("index","home");
	}
	public async Task<IActionResult> ReservationDetail(int id)
	{
		var response = await _httpClient.GetAsync(baseAddress + $"/reservations/getbyid/{id}");
		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dto = JsonConvert.DeserializeObject<ReservationGetByIdViewModel>(responseData);
			return View(dto);
		}
			return View();
	}

}


