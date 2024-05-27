using BookingProject.Domain.Entities;
using BookingProject.MVC.Models;
using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.HomeViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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
		var checkInDateString = HttpContext.Session.GetString("CheckInDate");
		var checkOutDateString = HttpContext.Session.GetString("CheckOutDate");
		if (!DateTime.TryParse(checkInDateString, out DateTime checkInDate) ||
			!DateTime.TryParse(checkOutDateString, out DateTime checkOutDate))
		{
			return BadRequest("Invalid date format.");
		}

		int numberOfNights = (int)(checkOutDate - checkInDate).TotalDays;
		ViewBag.Nights = numberOfNights;
		ViewBag.CheckInDate = checkInDate.ToString("yyyy-MM-dd");
		ViewBag.CheckOutDate = checkOutDate.ToString("yyyy-MM-dd");

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
		if (string.IsNullOrEmpty(dateRange))
		{
			return BadRequest("Date range is required.");
		}

		var dates = dateRange.Split(" to ");
		if (dates.Length != 2)
		{
			return BadRequest("Invalid date range format.");
		}

		if (!DateTime.TryParse(dates[0], out DateTime checkInDate) ||
			!DateTime.TryParse(dates[1], out DateTime checkOutDate) ||
			checkInDate >= checkOutDate)
		{
			return BadRequest("Invalid date range.");
		}

		HttpContext.Session.SetString("CheckInDate", checkInDate.ToString("yyyy-MM-dd"));
		HttpContext.Session.SetString("CheckOutDate", checkOutDate.ToString("yyyy-MM-dd"));

		if (!ModelState.IsValid) return View();

		var response = await _httpClient.GetAsync(baseAddress + "/hotels/getall");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotels = JsonConvert.DeserializeObject<List<HotelGetViewModel>>(responseData);
			var queryableHotels = hotels.Where(x => !x.IsDeactive).AsQueryable();

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

	public async Task<IActionResult> Reservation(int roomid,ReservationViewModel vm)
	{
		var roomResponse = await _httpClient.GetAsync(baseAddress + $"/rooms/getbyid/{roomid}");

		if (roomResponse.IsSuccessStatusCode)
		{
			var roomResponseData = await roomResponse.Content.ReadAsStringAsync();
			var room = JsonConvert.DeserializeObject<RoomGetViewModel>(roomResponseData);
			vm.Room = room;
			var checkInDateString = HttpContext.Session.GetString("CheckInDate");
			var checkOutDateString = HttpContext.Session.GetString("CheckOutDate");
			if (!DateTime.TryParse(checkInDateString, out DateTime checkInDate) ||
				!DateTime.TryParse(checkOutDateString, out DateTime checkOutDate))
			{
				return BadRequest("Invalid date format.");
			}

			int numberOfNights = (int)(checkOutDate - checkInDate).TotalDays;
			vm.Nights = numberOfNights;
			vm.CheckInDate = checkInDate.ToString("yyyy-MM-dd");
			vm.CheckOutDate = checkOutDate.ToString("yyyy-MM-dd");
			var hotelResponse = await _httpClient.GetAsync(baseAddress + $"/hotels/getbyid/{room.HotelId}");
			if (hotelResponse.IsSuccessStatusCode)
			{
				var hotelResponseData = await hotelResponse.Content.ReadAsStringAsync();
				var hotel = JsonConvert.DeserializeObject<HotelGetViewModel>(hotelResponseData);
				vm.Hotel = hotel;
			}
			var userResponse = await _httpClient.GetAsync(baseAddress + "/acc/getauthuser");

			if (userResponse.IsSuccessStatusCode)
			{
				var responseData = await userResponse.Content.ReadAsStringAsync();
				var user = JsonConvert.DeserializeObject<UserViewModel>(responseData);
				vm.User=user;
			}
			return View(vm);
		}
		return RedirectToAction("Index");
	}

	public async Task<IActionResult> ReserveRoom(int roomid, bool ispaid, ReservationCreateViewModel vm)
	{
		vm.RoomId=roomid;
		vm.IsPaid=ispaid;
		var checkInDateString = HttpContext.Session.GetString("CheckInDate");
		var checkOutDateString = HttpContext.Session.GetString("CheckOutDate");
		if (!DateTime.TryParse(checkInDateString, out DateTime checkInDate) ||
			!DateTime.TryParse(checkOutDateString, out DateTime checkOutDate))
		{
			return BadRequest("Invalid date format.");
		}
		vm.StartTime = checkInDate;
		vm.EndTime = checkOutDate;
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/reservations/create", stringContent);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Profile","Account");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return RedirectToAction("Index");
	}

}
