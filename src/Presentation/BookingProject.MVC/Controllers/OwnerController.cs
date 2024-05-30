using BookingProject.MVC.Models;
using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.AdminViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

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
    public async Task<IActionResult> Reviews(int itemPerPage=10,int page=1)
    {
        var response = await _httpClient.GetAsync(baseAddress + "/reviews/getallbyowner");

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
}
