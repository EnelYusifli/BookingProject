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
		return View();
	}
	[HttpGet]
	public async Task<IActionResult> HotelDetail([FromRoute]int id, string? dateRange)
	{
		HotelDetailViewModel vm = new();
		if (!ModelState.IsValid) return View();
        ViewBag.Dates = dateRange;
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
	public async Task<IActionResult> RoomDetail([FromRoute] int id,string? dateRange)
	{
		RoomGetViewModel vm = new();
        string? checkInDate = null;
        string? checkOutDate = null;

        if (!string.IsNullOrEmpty(dateRange))
        {
            var dates = dateRange.Split(" to ");
            if (dates.Length == 2)
            {
                checkInDate = dates[0];
                checkOutDate = dates[1];
            }
        }

        ViewBag.CheckInDate = checkInDate;
        ViewBag.CheckOutDate = checkOutDate;
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

	public async Task<IActionResult> HotelGrid(
		string? dateRange,
		decimal? minPrice,
		decimal? maxPrice,
		decimal? starPoint,
		string? typeName,
		bool? mostPopular,
		bool? mostRated,
		string? searchStr,
		int? adultCount,
		int? roomCount,
		int? childCount,
		string? countryName,
		int page = 1,
		int itemPerPage = 2)
	{
		DateTime? checkInDate = null;
		DateTime? checkOutDate = null;

		if (!string.IsNullOrEmpty(dateRange))
		{
			var dates = dateRange.Split(" to ");
			if (dates.Length == 2)
			{
				checkInDate = DateTime.Parse(dates[0]);
				checkOutDate = DateTime.Parse(dates[1]);
			}
		}
		ViewBag.Dates = dateRange;
		if (!ModelState.IsValid) return View();

		var response = await _httpClient.GetAsync(baseAddress + "/hotels/getall");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotels = JsonConvert.DeserializeObject<List<HotelGetViewModel>>(responseData);
			var queryableHotels = hotels.Where(x => x.IsDeactive == false).AsQueryable();

			if (!string.IsNullOrEmpty(searchStr))
			{
				queryableHotels = queryableHotels.Where(x => x.Name.Contains(searchStr) || x.Desc.Contains(searchStr) || x.Address.Contains(searchStr));
			}
			if (!string.IsNullOrEmpty(typeName))
			{
				queryableHotels = queryableHotels.Where(x => x.Name.ToLower() == typeName.ToLower());
			}

			if (minPrice.HasValue && maxPrice.HasValue)
			{
				queryableHotels = queryableHotels.Where(x => x.Rooms.Any(r => r.PricePerNight >= minPrice && r.PricePerNight <= maxPrice));
			}

			if (starPoint.HasValue)
			{
				queryableHotels = queryableHotels.Where(x => x.StarPoint >= starPoint);
			}

			//if (checkInDate.HasValue && checkOutDate.HasValue)
			//{
			//	queryableHotels = queryableHotels.Where(x => x.Rooms.Any(r =>
			//		!r.Reservations.Any(res =>
			//			(res.CheckInDate < checkOutDate && res.CheckOutDate > checkInDate))));
			//}

			if (adultCount.HasValue)
			{
				queryableHotels = queryableHotels.Where(x => x.Rooms.Any(r => r.AdultCount >= adultCount));
			}

			if (childCount.HasValue)
			{
				queryableHotels = queryableHotels.Where(x => x.Rooms.Any(r => r.ChildCount >= childCount));
			}

			if (roomCount.HasValue)
			{
				queryableHotels = queryableHotels.Where(x => x.Rooms.Count() >= roomCount);
			}

			if (!string.IsNullOrEmpty(countryName))
			{
				queryableHotels = queryableHotels.Where(x => x.CountryName.ToLower() == countryName.ToLower());
			}

			if (mostPopular.HasValue && mostPopular.Value)
			{
				queryableHotels = queryableHotels.OrderByDescending(x => x.ViewerCount);
			}

			if (mostRated.HasValue && mostRated.Value)
			{
				queryableHotels = queryableHotels.OrderByDescending(x => x.StarPoint);
			}

			var paginatedDatas = PaginatedList<HotelGetViewModel>.Create(queryableHotels, itemPerPage, page);

			return View(paginatedDatas);
		}
		return View();
	}


}
