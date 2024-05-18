using BookingProject.MVC.ViewModels.HomeViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookingProject.MVC.Controllers;

public class HomeController : Controller
{

	Uri baseAddress = new Uri("https://localhost:7197/api");
	private readonly HttpClient _httpClient;

	public HomeController(HttpClient httpClient)
	{
		_httpClient = httpClient;
		_httpClient.BaseAddress = baseAddress;
	}
	public async Task<IActionResult> Index()
	{
		HomeViewModel vm = new HomeViewModel();
		if (!ModelState.IsValid) return View();

		var response = await _httpClient.GetAsync(baseAddress + "/hotels/getall");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotels = JsonConvert.DeserializeObject<List<HotelGetViewModel>>(responseData);
			vm.Hotels = hotels.Where(x=>x.IsDeactive==false).OrderByDescending(h => h.StarPoint).Take(4).ToList();
			return View(vm);
		}
		return View();
	}
	[HttpGet]
	public async Task<IActionResult> HotelDetail([FromRoute]int id)
	{
		HotelDetailViewModel vm = new();
		if (!ModelState.IsValid) return View();

		var response = await _httpClient.GetAsync(baseAddress + $"/hotels/getbyid/{id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotel = JsonConvert.DeserializeObject<HotelGetViewModel>(responseData);
			vm.Hotel = hotel;
			return View(vm);
		}
		return RedirectToAction("Index");
	}
	[HttpGet]
	public async Task<IActionResult> RoomDetail([FromRoute] int id)
	{
		RoomGetViewModel vm = new();
		if (!ModelState.IsValid) return View();

		var response = await _httpClient.GetAsync(baseAddress + $"/rooms/getbyid/{id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var room = JsonConvert.DeserializeObject<RoomGetViewModel>(responseData);
			vm = room;
			return View(vm);
		}
		return RedirectToAction("Index");
	}
}
