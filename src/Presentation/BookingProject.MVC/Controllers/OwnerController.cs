using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookingProject.MVC.Controllers;

public class OwnerController : Controller
{
	Uri baseAddress = new Uri("https://localhost:7197/api");
	private readonly HttpClient _httpClient;

	public OwnerController(HttpClient httpClient)
	{
		_httpClient = httpClient;
		_httpClient.BaseAddress = baseAddress;
	}
	public IActionResult Dashboard()
	{
		return View();
	}
	public async Task<IActionResult> Listings()
	{
		var response = await _httpClient.GetAsync(baseAddress + "/hotels/getallbyuser");

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
		var response = await _httpClient.GetAsync(baseAddress + "/reservations/getallbyowner");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var dtos = JsonConvert.DeserializeObject<List<ReservationGetViewModel>>(responseData);
			dtos = dtos.Where(x => x.StartTime > DateTime.Now).ToList();
			return View(dtos);
		}
		return RedirectToAction("Index", "Home");
	}
    public async Task<IActionResult> Reviews()
    {
        var response = await _httpClient.GetAsync(baseAddress + "/reviews/getallbyowner");

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var dtos = JsonConvert.DeserializeObject<List<ReviewGetViewModel>>(responseData);
            return View(dtos);
        }
        return RedirectToAction("Index", "Home");
    }
}
