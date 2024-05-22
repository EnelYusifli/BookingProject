using BookingProject.MVC.Models;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace BookingProject.MVC.Controllers;

public class AdminController : Controller
{
	Uri baseAddress = new Uri("https://localhost:7197/api");
	private readonly HttpClient _httpClient;

	public AdminController(HttpClient httpClient)
	{
		_httpClient = httpClient;
		_httpClient.BaseAddress = baseAddress;
	}
	public IActionResult Index()
	{
		return View();
	}
	public async Task<IActionResult> SubmittedHotels(int itemPerPage=5,int page=1)
	{
		var response = await _httpClient.GetAsync(baseAddress + "/hotels/getall");
		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotels = JsonConvert.DeserializeObject<List<HotelGetViewModel>>(responseData);
			var queryableHotels = hotels.Where(x => x.IsDeactive == true && x.IsApproved == false).AsQueryable();
			var paginatedDatas = PaginatedList<HotelGetViewModel>.Create(queryableHotels, itemPerPage, page);

			return View(paginatedDatas);
		}
			return View();
	}
    public async Task<IActionResult> ApproveHotel(int id)
	{
        var response = await _httpClient.PutAsync(baseAddress + $"/hotels/approvehotel/{id}",null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("index","home");
        }
        return View();
    }
    public async Task<IActionResult> RefuseHotel(int id)
    {
        var response = await _httpClient.PutAsync(baseAddress + $"/hotels/refusehotel/{id}", null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("index", "home");
        }
        return View();
    }
	public async Task<IActionResult> HotelDetail(int id)
	{
		if (!ModelState.IsValid) return View();
		var response = await _httpClient.GetAsync(baseAddress + $"/hotels/getbyid/{id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotel = JsonConvert.DeserializeObject<HotelGetViewModel>(responseData);
			return View(hotel);
		}
		return RedirectToAction("Index");
	}
}
