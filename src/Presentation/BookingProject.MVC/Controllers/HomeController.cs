using BookingProject.MVC.Models;
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
	public IActionResult Index()
	{
		//HomeViewModel vm = new HomeViewModel();
		//if (!ModelState.IsValid) return View();

		//var response = await _httpClient.GetAsync(baseAddress + "/hotels/getall");

		//if (response.IsSuccessStatusCode)
		//{
		//	var responseData = await response.Content.ReadAsStringAsync();
		//	var hotels = JsonConvert.DeserializeObject<List<HotelGetViewModel>>(responseData);
		//	var queryableHotels = hotels.Where(x => x.Rooms.Any(x => x.IsCancellable)).AsQueryable();
		//	vm.Hotels = hotels.Where(x => x.IsDeactive == false).OrderByDescending(h => h.StarPoint).Take(4).ToList();
		//	var paginatedDatas = PaginatedList<HotelGetViewModel>.Create(queryableHotels, 5, 1);

		//	return View(vm);
		//}
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
	public async Task<IActionResult> HotelGrid(decimal? minPrice, decimal? maxPrice, decimal? starPoint, string? typeName, bool? mostPopular, bool? mostRated, string? searchStr, int page = 1, int itemPerPage = 2)
	{
		if (!ModelState.IsValid) return View();

		var response = await _httpClient.GetAsync(baseAddress + "/hotels/getall");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotels = JsonConvert.DeserializeObject<List<HotelGetViewModel>>(responseData);
			var queryableHotels = hotels.Where(x=>x.IsDeactive==false).AsQueryable();

			if (!string.IsNullOrEmpty(searchStr))
			{
				queryableHotels = queryableHotels.Where(x => x.Name.Contains(searchStr) || x.Desc.Contains(searchStr) || x.Address.Contains(searchStr));
			}
			if (!string.IsNullOrEmpty(typeName))
			{
				queryableHotels = queryableHotels.Where(x => x.Name.ToLower()==typeName.ToLower());
			}

			if (minPrice.HasValue && maxPrice.HasValue)
			{
				queryableHotels = queryableHotels.Where(x => x.Rooms.Any(r => r.PricePerNight >= minPrice && r.PricePerNight <= maxPrice));
			}

			if (starPoint.HasValue)
			{
				queryableHotels = queryableHotels.Where(x => x.StarPoint>=starPoint);
			}

			//if (isFeat.HasValue && isFeat.Value)
			//{
			//	queryableHotels = queryableHotels.Where(x => x.Rooms.Any(r => r.IsFeatured));
			//}

			//if (isNew.HasValue && isNew.Value)
			//{
			//	queryableHotels = queryableHotels.Where(x => x.Rooms.Any(r => r.IsNew));
			//}

			//if (isBest.HasValue && isBest.Value)
			//{
			//	queryableHotels = queryableHotels.Where(x => x.Rooms.Any(r => r.IsBestSeller));
			//}
			if(mostPopular.HasValue && mostPopular==true)
			queryableHotels = queryableHotels.Where(x => !x.IsDeactive).OrderByDescending(x => x.ViewerCount);
			if (mostRated.HasValue && mostRated == true)
				queryableHotels = queryableHotels.Where(x => !x.IsDeactive).OrderByDescending(x => x.StarPoint);
			var paginatedDatas = PaginatedList<HotelGetViewModel>.Create(queryableHotels, itemPerPage, page);

			return View(paginatedDatas);
		}
		return View();
	}

}
