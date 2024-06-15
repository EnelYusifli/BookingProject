using BookingProject.Domain.Entities;
using BookingProject.MVC.Models;
using BookingProject.MVC.Services;
using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.AdminViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.RoomViewModels;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace BookingProject.MVC.Controllers;
[Authorize(Roles = "Owner")]
public class OwnerController : Controller
{
	Uri baseAddress = new Uri("https://localhost:7197/api");
	private readonly HttpClient _httpClient;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OwnerController(HttpClient httpClient, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _userManager = userManager;
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
    public async Task<IActionResult> Index()
	{
		AppUser user = await GetCurrentUserAsync();
		var response = await _httpClient.GetAsync(baseAddress + $"/reservations/getallbyowner/{user.Id}");
		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dtos = JsonConvert.DeserializeObject<List<ReservationGetViewModel>>(responseData);
			ViewBag.ReservationCount = dtos.Count();
		}
		var response2 = await _httpClient.GetAsync(baseAddress + $"/hotels/getallbyuser/{user.Id}");
		if (response2.IsSuccessStatusCode)
		{
			var responseData = await response2.Content.ReadAsStringAsync();
			var dtos = JsonConvert.DeserializeObject<List<ReservationGetViewModel>>(responseData);
			ViewBag.ListingCount = dtos.Count();
		}
		var response3 = await _httpClient.GetAsync(baseAddress + $"/reviews/getallbyowner/{user.Id}");
		if (response3.IsSuccessStatusCode)
		{
			var responseData = await response3.Content.ReadAsStringAsync();
			var dtos = JsonConvert.DeserializeObject<List<ReservationGetViewModel>>(responseData);
			ViewBag.ReviewCount = dtos.Count();
		}
		return View();
	}
	public async Task<IActionResult> Listings()
	{
        AppUser user = await GetCurrentUserAsync();
		var response = await _httpClient.GetAsync(baseAddress + $"/hotels/getallbyuser/{user.Id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotels = JsonConvert.DeserializeObject<List<OwnerHotelGetViewModel>>(responseData);
			return View(hotels);
		}
		return RedirectToAction("Index");
	}
	public async Task<IActionResult> Reservations(int? select)
	{
        AppUser user = await GetCurrentUserAsync();
		var response = await _httpClient.GetAsync(baseAddress + $"/reservations/getallbyowner/{user.Id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dtos = JsonConvert.DeserializeObject<List<ReservationGetViewModel>>(responseData);
			if(select is not null && select == 2) 
				dtos=dtos.OrderBy(x=>x.StartTime).ToList();
			if(select is not null && select == 3) 
				dtos=dtos.Where(item=> (!item.IsCancelled) && item.StartTime > DateTime.Now).ToList();
			if(select is not null && select == 4) 
				dtos=dtos.Where(item=> (!item.IsCancelled) && item.StartTime < DateTime.Now && item.EndTime > DateTime.Now).ToList();
			if(select is not null && select == 5) 
				dtos=dtos.Where(item=> (!item.IsCancelled)).ToList();
			//dtos = dtos.Where(x => x.StartTime > DateTime.Now).ToList();
			return View(dtos);
		}
		return RedirectToAction("Index", "Home");
	}
    public async Task<IActionResult> Reviews(int itemPerPage=10,int page=1)
    {
        AppUser user = await GetCurrentUserAsync();
        var response = await _httpClient.GetAsync(baseAddress + $"/reviews/getallbyowner/{user.Id}");

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var dtos = JsonConvert.DeserializeObject<List<ReviewGetViewModel>>(responseData);
			dtos=dtos.Where(x=>x.IsReported==false && x.IsDeactive==false).ToList();
			var queryableItems = dtos.AsQueryable();
			var paginatedDatas = PaginatedList<ReviewGetViewModel>.Create(queryableItems, itemPerPage, page);
			return View(paginatedDatas);
        }
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> ReportReview(int id)
    {
        var response = await _httpClient.PutAsync($"{baseAddress}/reviews/report/{id}", null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("reviews");
        }
        return RedirectToAction("Index", "Home");
    }
	[Authorize(Roles = "Owner")]
	public async Task<IActionResult> CancelReservation(int reservationId)
	{
		var response = await _httpClient.PutAsync(baseAddress + $"/reservations/cancel/{reservationId}", null);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Reservations");
		}
		return RedirectToAction("Index", "Home");
	}
	//[Authorize(Roles = "Owner")]
	//public IActionResult CreateDiscount()
	//{
	//	return View();
	//}
	[Authorize(Roles = "Owner")]
	[HttpPost]
	public async Task<IActionResult> CreateDiscount(DiscountCreateViewModel vm)
	{
		if (!ModelState.IsValid) return View();
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + $"/discounts/create/{vm.RoomId}", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("index");
		}
		return RedirectToAction("Index", "Home");
	}
	public async Task<IActionResult> HotelRooms(int hotelid)
	{
		var response = await _httpClient.GetAsync(baseAddress + $"/rooms/getall/{hotelid}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dtos = JsonConvert.DeserializeObject<List<RoomGetViewModel>>(responseData);
			return View(dtos);
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return RedirectToAction("Index", "Home");
	}
}
