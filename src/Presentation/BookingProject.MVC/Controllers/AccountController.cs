using BookingProject.Domain.Entities;
using BookingProject.MVC.Models;
using BookingProject.MVC.Services;
using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.HomeViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.ProfileViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<AppUser> _userManager;
    private readonly ILoginService _loginService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(HttpClient httpClient,UserManager<AppUser> userManager,ILoginService loginService,IHttpContextAccessor httpContextAccessor)
	{
		_httpClient = httpClient;
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
    public IActionResult Login()
	{
		return View();
	}
	[HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid)
            return View();
		try
		{
		await _loginService.LoginUser(vm);
            return RedirectToAction("Index", "Home");

        }
		catch (Exception ex)
		{
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
	[HttpPost]
	public async Task<IActionResult> UpdatePersonalInfo(ProfileViewModel vm)
	{
        using (var content = new MultipartFormDataContent())
        {
            AppUser user = await GetCurrentUserAsync();
			vm.PersonalInfo.Id = user.Id;
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
			var queryableItems = vms.AsQueryable();
			var paginatedDatas = PaginatedList<WishlistViewModel>.Create(queryableItems, itemPerPage, page);
			return View(paginatedDatas);
		}
		return RedirectToAction("Index");
	}
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
	public IActionResult LeaveReview(int hotelid)
	{
		ViewBag.Id=hotelid;
		return View();
	}
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
}


