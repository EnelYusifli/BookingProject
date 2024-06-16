using BookingProject.Domain.Entities;
using BookingProject.MVC.Models;
using BookingProject.MVC.ViewModels.AdminViewModels;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.About;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.FAQ;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.TermsOfService;
using BookingProject.MVC.ViewModels.HomeViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.PropertyViewModels;
using BookingProject.MVC.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BookingProject.MVC.Controllers;

public class HomeController : Controller
{

	Uri baseAddress = new Uri("https://localhost:7197/api");
	private readonly HttpClient _httpClient;
    private readonly UserManager<AppUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IHttpContextAccessor _httpContextAccessor;

    public HomeController(HttpClient httpClient, UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _userManager = userManager;
		_roleManager = roleManager;
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
		var response = await _httpClient.GetAsync(baseAddress + "/countries/getall");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var countries = JsonConvert.DeserializeObject<List<GetCountryViewModel>>(responseData);
			ViewBag.Countries = countries;	
		}
		var response2 = await _httpClient.GetAsync(baseAddress + "/types/getall");

		if (response2.IsSuccessStatusCode)
		{
			var responseData2 = await response2.Content.ReadAsStringAsync();
			var types = JsonConvert.DeserializeObject<List<GetTypeViewModel>>(responseData2);
			ViewBag.Types = types;	
		}
		return View();

	}
	private async Task PopulatePropertyViewModel(PropertyViewModel propertyViewModel)
	{
		var typesResponse = await _httpClient.GetAsync(baseAddress + "/types/getall");
		if (typesResponse.IsSuccessStatusCode)
		{
			var typesData = await typesResponse.Content.ReadAsStringAsync();
			propertyViewModel.Types = JsonConvert.DeserializeObject<List<TypeGetViewModel>>(typesData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve types data.");
		}
		var countriesResponse = await _httpClient.GetAsync(baseAddress + "/countries/getall");
		if (countriesResponse.IsSuccessStatusCode)
		{
			var countriesData = await countriesResponse.Content.ReadAsStringAsync();
			propertyViewModel.Countries = JsonConvert.DeserializeObject<List<CountriesGetViewModel>>(countriesData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve countries data.");
		}

		var servicesResponse = await _httpClient.GetAsync(baseAddress + "/services/getall");
		if (servicesResponse.IsSuccessStatusCode)
		{
			var servicesData = await servicesResponse.Content.ReadAsStringAsync();
			propertyViewModel.Services = JsonConvert.DeserializeObject<List<ServiceGetViewModel>>(servicesData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve services data.");
		}
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
            var incrementResponse = await _httpClient.PostAsync(baseAddress + $"/hotels/IncrementViewerCount/{id}", null);
            if (!incrementResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to update viewer count");
            }
            return View(vm);
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("Error response from API:");
			Console.WriteLine(responseContent);
		}
		return RedirectToAction("Index");
	}
	[HttpGet]
	public async Task<IActionResult> RoomDetail([FromRoute] int id)
	{
		RoomGetViewModel vm = new();
		var checkInDateString = HttpContext.Session.GetString("CheckInDate");
		var checkOutDateString = HttpContext.Session.GetString("CheckOutDate");
		if (string.IsNullOrEmpty(checkInDateString))
		{
			checkInDateString = $"{DateTime.Now.ToString("yyyy-MM-dd")}";
		}if (string.IsNullOrEmpty(checkOutDateString))
		{
			checkOutDateString = $"{DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")}";
		}
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
		decimal? minPrice,
		decimal? maxPrice,
		decimal? starPoint,
		string? typeName,
		string[]? serviceNames,
		int? select,
		string? searchStr,
		int? roomCount,
		string? countryName,
		string? dateRange,
		int? childCount=0,
		int? adultCount=1,
		int page = 1,
		int itemPerPage = 3)
	{
		if (string.IsNullOrEmpty(dateRange))
		{
			dateRange = $"{DateTime.Now:yyyy-MM-dd} to {DateTime.Now.AddDays(2):yyyy-MM-dd}";
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
		HttpContext.Session.SetString("ChildCount", childCount.ToString());
		HttpContext.Session.SetString("AdultCount", adultCount.ToString());

		if (!ModelState.IsValid) return View();

		var response = await _httpClient.GetAsync(baseAddress + "/hotels/getall");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var hotels = JsonConvert.DeserializeObject<List<HotelGetViewModel>>(responseData);
			var queryableHotels = hotels.Where(x => (!x.IsDeactive) && x.IsApproved).AsQueryable();

			if (!string.IsNullOrEmpty(searchStr))
			{
				queryableHotels = queryableHotels.Where(x => x.Name.ToLower().Contains(searchStr.ToLower()));
			}
			if (!string.IsNullOrEmpty(typeName))
			{
				queryableHotels = queryableHotels.Where(x => x.TypeName.ToLower() == typeName.ToLower());
			}
			if (minPrice.HasValue && maxPrice.HasValue)
			{
				queryableHotels = queryableHotels.Where(x => x.Rooms.Any(r => r.PricePerNight >= minPrice && r.PricePerNight <= maxPrice));
			}
			if (starPoint.HasValue)
			{
				queryableHotels = queryableHotels.Where(x => x.StarPoint >= starPoint);
			}

			if (roomCount.HasValue && (adultCount.HasValue || childCount.HasValue))
			{
				int totalAdults = adultCount ?? 0;
				int totalChildren = childCount ?? 0;
				int requiredGuests = totalAdults + totalChildren;

				queryableHotels = queryableHotels.Where(hotel => HotelHasRequiredRooms(hotel, roomCount.Value, checkInDate, checkOutDate, requiredGuests));
			}


			if (!string.IsNullOrEmpty(countryName))
			{
				queryableHotels = queryableHotels.Where(x => x.CountryName.ToLower() == countryName.ToLower());
			}
			if (select.HasValue && select==1)
			{
				queryableHotels = queryableHotels.OrderByDescending(x => x.ViewerCount);
			}
			if (serviceNames is not null && serviceNames.Count() > 0 )
			{
				foreach (var item in serviceNames)
				{
				queryableHotels = queryableHotels.Where(x => x.ServiceNames.Any(x=> x.ToLower() == item.ToLower()));
				}
			}
			if (select.HasValue && select == 2)
			{
				queryableHotels = queryableHotels.OrderByDescending(x => x.StarPoint);
			}
			PropertyViewModel viewModel = new();
			await PopulatePropertyViewModel(viewModel);
			ViewBag.Property = viewModel;
			var paginatedDatas = PaginatedList<HotelGetViewModel>.Create(queryableHotels, itemPerPage, page);

			return View(paginatedDatas);
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("Error response from API:");
			Console.WriteLine(responseContent);
		}
		return View();
	}
	private bool HotelHasRequiredRooms(HotelGetViewModel hotel, int roomCount, DateTime checkInDate, DateTime checkOutDate, int requiredGuests)
	{
		var availableRooms = hotel.Rooms.Where(room =>
			!room.Reservations.Any(res => res.StartTime < checkOutDate && res.EndTime > checkInDate && !res.IsDeactive)).ToList();

		if (availableRooms.Count < roomCount)
		{
			return false;
		}

		return CanAccommodateGuests(availableRooms, roomCount, requiredGuests);
	}
	private bool CanAccommodateGuests(List<RoomGetViewModel> availableRooms, int roomCount, int requiredGuests)
	{
		var sortedRooms = availableRooms.OrderByDescending(r => r.AdultCount + r.ChildCount).ToList();

		for (int i = 0; i <= sortedRooms.Count - roomCount; i++)
		{
			int currentCapacity = 0;
			for (int j = 0; j < roomCount; j++)
			{
				currentCapacity += sortedRooms[i + j].AdultCount + sortedRooms[i + j].ChildCount;
			}
			if (currentCapacity >= requiredGuests)
			{
				return true;
			}
		}

		return false;
	}



	[Authorize(Roles = "Customer,Owner,Admin")]
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
			var adultCount = HttpContext.Session.GetString("AdultCount");
			var childCount = HttpContext.Session.GetString("ChildCount");
			if (string.IsNullOrEmpty(checkInDateString))
			{
				checkInDateString = $"{DateTime.Now.ToString("yyyy-MM-dd")}";
			}
			if (string.IsNullOrEmpty(checkOutDateString))
			{
				checkOutDateString = $"{DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")}";
			}
			if (!DateTime.TryParse(checkInDateString, out DateTime checkInDate) ||
				!DateTime.TryParse(checkOutDateString, out DateTime checkOutDate))
			{
				return BadRequest("Invalid date format.");
			}

			int numberOfNights = (int)(checkOutDate - checkInDate).TotalDays;
			vm.Nights = numberOfNights;
			vm.AdultCount = adultCount;
			vm.ChildCount = childCount;
			vm.CheckInDate = checkInDate.ToString("yyyy-MM-dd");
			vm.CheckOutDate = checkOutDate.ToString("yyyy-MM-dd");
			var hotelResponse = await _httpClient.GetAsync(baseAddress + $"/hotels/getbyid/{room.HotelId}");
			if (hotelResponse.IsSuccessStatusCode)
			{
				var hotelResponseData = await hotelResponse.Content.ReadAsStringAsync();
				var hotel = JsonConvert.DeserializeObject<HotelGetViewModel>(hotelResponseData);
				vm.Hotel = hotel;
			}
			return View(vm);
		}
		return RedirectToAction("Index");
	}
    [Authorize(Roles = "Customer,Owner,Admin")]
    public async Task<IActionResult> ReserveRoom(int roomid, bool ispaid, ReservationCreateViewModel vm)
	{
		vm.RoomId=roomid;
		vm.IsPaid=ispaid;
		AppUser user = await GetCurrentUserAsync();
		vm.UserId=user.Id;
		var checkInDateString = HttpContext.Session.GetString("CheckInDate");
		var checkOutDateString = HttpContext.Session.GetString("CheckOutDate");
		if (string.IsNullOrEmpty(checkInDateString))
		{
			checkInDateString = $"{DateTime.Now.ToString("yyyy-MM-dd")}";
		}
		if (string.IsNullOrEmpty(checkOutDateString))
		{
			checkOutDateString = $"{DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")}";
		}
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
			return RedirectToAction("Reservations","Account");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return RedirectToAction("Index");
	}
	public async Task<IActionResult> About()
	{
		ViewBag.CustomerCount = _userManager.Users.Count();
		var usersWithOwnerRole = await GetUsersInRoleAsync("Owner");
		ViewBag.OwnerCount = usersWithOwnerRole.Count();
        var response = await _httpClient.GetAsync($"{baseAddress}/about/getbyid");
        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Home");
        }

        var dataStr = await response.Content.ReadAsStringAsync();
        var vm = JsonConvert.DeserializeObject<UpdateAboutViewModel>(dataStr);

        return View(vm);
    }
	public async Task<IActionResult> TermsOfService()
	{
		var response = await _httpClient.GetAsync($"{baseAddress}/termsofservice/getbyid");
		if (!response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index", "Home");
		}

		var dataStr = await response.Content.ReadAsStringAsync();
		var vm = JsonConvert.DeserializeObject<UpdateTermsOfServiceViewModel>(dataStr);

		return View(vm);
	}
	private async Task<List<AppUser>> GetUsersInRoleAsync(string roleName)
	{
		var role = await _roleManager.FindByNameAsync(roleName);
		if (role == null)
		{
			return new List<AppUser>();
		}

		var userIds = await _userManager.GetUsersInRoleAsync(roleName);
		var users = new List<AppUser>();

		foreach (var user in userIds)
		{
			users.Add(await _userManager.FindByIdAsync(user.Id));
		}

		return users;
	}
	public IActionResult Contact()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> SendMessage(MessageViewModel vm)
	{
		//AppUser user= await GetCurrentUserAsync();
		//ViewBag.Email=user.Email;
		//ViewBag.Name=user.FirstName;
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/messages/create", stringContent);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Contact");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return RedirectToAction("Index");
	}
	public IActionResult PrivacyPolicy()
	{
		return View();
	}
	public async Task<IActionResult> FAQs()
	{
        var response = await _httpClient.GetAsync($"{baseAddress}/faqs/getall");
        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Home");
        }

        var dataStr = await response.Content.ReadAsStringAsync();
        var vm = JsonConvert.DeserializeObject<List<FAQGetViewModel>>(dataStr);

        return View(vm);
    }
}
public static class EnumerableExtensions
{
	public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
	{
		return k == 0 ? new[] { new T[0] } :
			elements.SelectMany((e, i) =>
				elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
	}
}
