using BookingProject.Domain.Entities;
using BookingProject.MVC.Models;
using BookingProject.MVC.Services;
using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.AdminViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

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
    public async Task<IActionResult> Dashboard()
	{
		AppUser user = await GetCurrentUserAsync();
		var response = await _httpClient.GetAsync(baseAddress + $"/reservations/getallbyowner/{user.Id}");
		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dtos = JsonConvert.DeserializeObject<List<ReservationGetViewModel>>(responseData);
			dtos = dtos.Where(x => x.StartTime > DateTime.Now).ToList();
			return View(dtos);
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
		return RedirectToAction("Dashboard");
	}
	public async Task<IActionResult> Reservations()
	{
        AppUser user = await GetCurrentUserAsync();
		var response = await _httpClient.GetAsync(baseAddress + $"/reservations/getallbyowner/{user.Id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dtos = JsonConvert.DeserializeObject<List<ReservationGetViewModel>>(responseData);
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
}
