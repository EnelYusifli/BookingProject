using BookingProject.MVC.Models;
using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.AdminViewModels;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.About;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Activity;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Country;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.FAQ;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Message;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.PaymentMethod;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Service;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.StaffLanguage;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.TermsOfService;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Type;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http;
using System.Text;

namespace BookingProject.MVC.Controllers;
[Authorize(Roles = "Admin")]
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
	public async Task<IActionResult> OwnersList(int itemPerPage = 5, int page = 1)
	{
		var response = await _httpClient.GetAsync(baseAddress + "/users/getall");
		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var users = JsonConvert.DeserializeObject<List<ViewModels.AdminViewModels.UserGetViewModel>>(responseData);
			var queryableUsers = users.Where(x => x.Roles.Contains("Owner")).AsQueryable();
			var paginatedDatas = PaginatedList<ViewModels.AdminViewModels.UserGetViewModel>.Create(queryableUsers, itemPerPage, page);

			return View(paginatedDatas);
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return View();
	}
	public async Task<IActionResult> OwnerDetail(string id)
	{
		UserDetailViewModel vm = new();
		if (!ModelState.IsValid) return View();
		var response = await _httpClient.GetAsync(baseAddress + $"/users/getbyid/{id}");

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var user = JsonConvert.DeserializeObject<UserDetailViewModel>(responseData);
			return View(user);
		}
		return RedirectToAction("Index");
	}

	public async Task<IActionResult> UserHotelDetail(int id)
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

	public async Task<IActionResult> Activities(int itemPerPage = 5, int page = 1)
	{
		var response = await _httpClient.GetAsync(baseAddress + "/activities/getall");
		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var act = JsonConvert.DeserializeObject<List<GetActivityViewModel>>(responseData);
			var queryableItems = act.AsQueryable();
			var paginatedDatas = PaginatedList<GetActivityViewModel>.Create(queryableItems, itemPerPage, page);

			return View(paginatedDatas);
		}
		return View();
	}
	public async Task<IActionResult> CreateActivity()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> CreateActivity(CreateActivityViewModel vm)
	{
		if (!ModelState.IsValid) return View();
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/activities/create", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("activities");
		}
		return View();
	}
    [HttpGet]
    public async Task<IActionResult> UpdateActivity(int id)
    {
        var response = await _httpClient.GetAsync($"{baseAddress}/activities/getbyid/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var dataStr = await response.Content.ReadAsStringAsync();
        var vm = JsonConvert.DeserializeObject<UpdateActivityViewModel>(dataStr);

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateActivity(int id, UpdateActivityViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
		vm.Id = id;
        var dataStr = JsonConvert.SerializeObject(vm);
        var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{baseAddress}/activities/update/{id}", stringContent);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("activities");
        }
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return View(vm);
    }

    public async Task<IActionResult> DeleteActivity(int id)
	{
		if (!ModelState.IsValid) return View();
		var response = await _httpClient.DeleteAsync(baseAddress + $"/activities/delete/{id}");

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("activities");
		}
		return View();
	}
	public async Task<IActionResult> SoftDeleteActivity(int id)
	{
		if (!ModelState.IsValid) return View();
		var response = await _httpClient.PutAsync(baseAddress + $"/activities/softdelete/{id}", null);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("activities");
		}
		return View();
	}
	public async Task<IActionResult> CreateCountry()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> CreateCountry(CreateCountryViewModel vm)
	{
		if (!ModelState.IsValid) return View();
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/countries/create", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("countries");
		}
		return View();
	}
	[HttpGet]
	public async Task<IActionResult> UpdateCountry(int id)
	{
		var response = await _httpClient.GetAsync($"{baseAddress}/countries/getbyid/{id}");
		if (!response.IsSuccessStatusCode)
		{
			return NotFound();
		}

		var dataStr = await response.Content.ReadAsStringAsync();
		var vm = JsonConvert.DeserializeObject<UpdateCountryViewModel>(dataStr);

		return View(vm);
	}

	[HttpPost]
	public async Task<IActionResult> UpdateCountry(int id, UpdateCountryViewModel vm)
	{
		if (!ModelState.IsValid) return View(vm);
		vm.Id = id;
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync($"{baseAddress}/countries/update/{id}", stringContent);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("countries");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return View(vm);
	}

	public async Task<IActionResult> DeleteCountry(int id)
	{
		if (!ModelState.IsValid) return View();
		var response = await _httpClient.DeleteAsync(baseAddress + $"/countries/delete/{id}");

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("countries");
		}
		return View();
	}
	public async Task<IActionResult> SoftDeleteCountry(int id)
	{
		if (!ModelState.IsValid) return View();
		var response = await _httpClient.PutAsync(baseAddress + $"/countries/softdelete/{id}", null);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("countries");
		}
		return View();
	}
	public async Task<IActionResult> Countries(int itemPerPage = 5, int page = 1)
	{
		var response = await _httpClient.GetAsync(baseAddress + "/Countries/getall");
		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var act = JsonConvert.DeserializeObject<List<GetCountryViewModel>>(responseData);
			var queryableItems = act.AsQueryable();
			var paginatedDatas = PaginatedList<GetCountryViewModel>.Create(queryableItems, itemPerPage, page);

			return View(paginatedDatas);
		}
		return View();
	}
	public async Task<IActionResult> CreatePaymentMethod()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> CreatePaymentMethod(CreatePaymentMethodViewModel vm)
	{
		if (!ModelState.IsValid) return View();
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/paymentmethods/create", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("paymentmethods");
		}
		return View();
	}
	[HttpGet]
	public async Task<IActionResult> UpdatePaymentMethod(int id)
	{
		var response = await _httpClient.GetAsync($"{baseAddress}/paymentmethods/getbyid/{id}");
		if (!response.IsSuccessStatusCode)
		{
			return NotFound();
		}

		var dataStr = await response.Content.ReadAsStringAsync();
		var vm = JsonConvert.DeserializeObject<UpdatePaymentMethodViewModel>(dataStr);

		return View(vm);
	}

	[HttpPost]
	public async Task<IActionResult> UpdatePaymentMethod(int id, UpdatePaymentMethodViewModel vm)
	{
		if (!ModelState.IsValid) return View(vm);
		vm.Id = id;
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync($"{baseAddress}/paymentmethods/update/{id}", stringContent);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("paymentmethods");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return View(vm);
	}

	public async Task<IActionResult> DeletePaymentMethod(int id)
	{
		if (!ModelState.IsValid) return View();
		var response = await _httpClient.DeleteAsync(baseAddress + $"/paymentmethods/delete/{id}");

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("paymentmethods");
		}
		return View();
	}
	public async Task<IActionResult> SoftDeletePaymentMethod(int id)
	{
		if (!ModelState.IsValid) return View();
		var response = await _httpClient.PutAsync(baseAddress + $"/paymentmethods/softdelete/{id}", null);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("paymentmethods");
		}
		return View();
	}
	public async Task<IActionResult> PaymentMethods(int itemPerPage = 5, int page = 1)
	{
		var response = await _httpClient.GetAsync(baseAddress + "/paymentmethods/getall");
		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var act = JsonConvert.DeserializeObject<List<GetPaymentMethodViewModel>>(responseData);
			var queryableItems = act.AsQueryable();
			var paginatedDatas = PaginatedList<GetPaymentMethodViewModel>.Create(queryableItems, itemPerPage, page);

			return View(paginatedDatas);
		}
		return View();
	}
    public async Task<IActionResult> CreateService()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateService(CreateServiceViewModel vm)
    {
        if (!ModelState.IsValid) return View();
        var dataStr = JsonConvert.SerializeObject(vm);
        var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(baseAddress + "/Services/create", stringContent);


        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Services");
        }
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> UpdateService(int id)
    {
        var response = await _httpClient.GetAsync($"{baseAddress}/Services/getbyid/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var dataStr = await response.Content.ReadAsStringAsync();
        var vm = JsonConvert.DeserializeObject<UpdateServiceViewModel>(dataStr);

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateService(int id, UpdateServiceViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        vm.Id = id;
        var dataStr = JsonConvert.SerializeObject(vm);
        var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{baseAddress}/Services/update/{id}", stringContent);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Services");
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Console.WriteLine(responseContent);
        }
        return View(vm);
    }

    public async Task<IActionResult> DeleteService(int id)
    {
        if (!ModelState.IsValid) return View();
        var response = await _httpClient.DeleteAsync(baseAddress + $"/Services/delete/{id}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Services");
        }
        return View();
    }
    public async Task<IActionResult> SoftDeleteService(int id)
    {
        if (!ModelState.IsValid) return View();
        var response = await _httpClient.PutAsync(baseAddress + $"/Services/softdelete/{id}", null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Services");
        }
        return View();
    }
    public async Task<IActionResult> Services(int itemPerPage = 5, int page = 1)
    {
        var response = await _httpClient.GetAsync(baseAddress + "/Services/getall");
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var act = JsonConvert.DeserializeObject<List<GetServiceViewModel>>(responseData);
            var queryableItems = act.AsQueryable();
            var paginatedDatas = PaginatedList<GetServiceViewModel>.Create(queryableItems, itemPerPage, page);

            return View(paginatedDatas);
        }
        return View();
    }
    public async Task<IActionResult> CreateStaffLanguage()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateStaffLanguage(CreateStaffLanguageViewModel vm)
    {
        if (!ModelState.IsValid) return View();
        var dataStr = JsonConvert.SerializeObject(vm);
        var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(baseAddress + "/StaffLanguages/create", stringContent);


        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("StaffLanguages");
        }
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> UpdateStaffLanguage(int id)
    {
        var response = await _httpClient.GetAsync($"{baseAddress}/StaffLanguages/getbyid/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var dataStr = await response.Content.ReadAsStringAsync();
        var vm = JsonConvert.DeserializeObject<UpdateStaffLanguageViewModel>(dataStr);

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStaffLanguage(int id, UpdateStaffLanguageViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        vm.Id = id;
        var dataStr = JsonConvert.SerializeObject(vm);
        var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{baseAddress}/StaffLanguages/update/{id}", stringContent);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("StaffLanguages");
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Console.WriteLine(responseContent);
        }
        return View(vm);
    }

    public async Task<IActionResult> DeleteStaffLanguage(int id)
    {
        if (!ModelState.IsValid) return View();
        var response = await _httpClient.DeleteAsync(baseAddress + $"/StaffLanguages/delete/{id}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("StaffLanguages");
        }
        return View();
    }
    public async Task<IActionResult> SoftDeleteStaffLanguage(int id)
    {
        if (!ModelState.IsValid) return View();
        var response = await _httpClient.PutAsync(baseAddress + $"/StaffLanguages/softdelete/{id}", null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("StaffLanguages");
        }
        return View();
    }
    public async Task<IActionResult> StaffLanguages(int itemPerPage = 5, int page = 1)
    {
        var response = await _httpClient.GetAsync(baseAddress + "/StaffLanguages/getall");
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var act = JsonConvert.DeserializeObject<List<GetStaffLanguageViewModel>>(responseData);
            var queryableItems = act.AsQueryable();
            var paginatedDatas = PaginatedList<GetStaffLanguageViewModel>.Create(queryableItems, itemPerPage, page);

            return View(paginatedDatas);
        }
        return View();
    }
    public async Task<IActionResult> CreateType()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateType(CreateTypeViewModel vm)
    {
        if (!ModelState.IsValid) return View();
        var dataStr = JsonConvert.SerializeObject(vm);
        var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(baseAddress + "/Types/create", stringContent);


        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Types");
        }
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> UpdateType(int id)
    {
        var response = await _httpClient.GetAsync($"{baseAddress}/Types/getbyid/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var dataStr = await response.Content.ReadAsStringAsync();
        var vm = JsonConvert.DeserializeObject<UpdateTypeViewModel>(dataStr);

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateType(int id, UpdateTypeViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        vm.Id = id;
        var dataStr = JsonConvert.SerializeObject(vm);
        var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{baseAddress}/Types/update/{id}", stringContent);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Types");
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Console.WriteLine(responseContent);
        }
        return View(vm);
    }

    public async Task<IActionResult> DeleteType(int id)
    {
        if (!ModelState.IsValid) return View();
        var response = await _httpClient.DeleteAsync(baseAddress + $"/Types/delete/{id}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Types");
        }
        return View();
    }
    public async Task<IActionResult> SoftDeleteType(int id)
    {
        if (!ModelState.IsValid) return View();
        var response = await _httpClient.PutAsync(baseAddress + $"/Types/softdelete/{id}", null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Types");
        }
        return View();
    }
    public async Task<IActionResult> Types(int itemPerPage = 5, int page = 1)
    {
        var response = await _httpClient.GetAsync(baseAddress + "/Types/getall");
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var act = JsonConvert.DeserializeObject<List<GetTypeViewModel>>(responseData);
            var queryableItems = act.AsQueryable();
            var paginatedDatas = PaginatedList<GetTypeViewModel>.Create(queryableItems, itemPerPage, page);

            return View(paginatedDatas);
        }
        return View();
    }
    public async Task<IActionResult> ReportedReviews(int itemPerPage = 10, int page = 1)
    {
        var response = await _httpClient.GetAsync(baseAddress + "/reviews/getall");

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var dtos = JsonConvert.DeserializeObject<List<ReviewGetViewModel>>(responseData);
            dtos = dtos.Where(x => x.IsReported == true).ToList();
            var queryableItems = dtos.AsQueryable();
            var paginatedDatas = PaginatedList<ReviewGetViewModel>.Create(queryableItems, itemPerPage, page);
            return View(paginatedDatas);
        }
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> DeleteReview(int id)
    {
        var response = await _httpClient.DeleteAsync(baseAddress + $"/reviews/delete/{id}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ReportedReviews");
        }
            return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> NonReportReview(int id)
    {
        var response = await _httpClient.PutAsync($"{baseAddress}/reviews/report/{id}", null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("reportedreviews");
        }
        return RedirectToAction("Index", "Home");
    }
	[HttpGet]
	public async Task<IActionResult> UpdateAbout()
	{
		var response = await _httpClient.GetAsync($"{baseAddress}/about/getbyid");
		if (!response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index","Home");
		}

		var dataStr = await response.Content.ReadAsStringAsync();
		var vm = JsonConvert.DeserializeObject<UpdateAboutViewModel>(dataStr);

		return View(vm);
	}
	[HttpPost]
	public async Task<IActionResult> UpdateAbout(UpdateAboutViewModel vm)
	{
		if (!ModelState.IsValid) return View(vm);
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync($"{baseAddress}/about/update", stringContent);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return View(vm);
	}

	[HttpGet]
	public async Task<IActionResult> UpdateTermsOfService()
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
	[HttpPost]
	public async Task<IActionResult> UpdateTermsOfService(UpdateTermsOfServiceViewModel vm)
	{
		if (!ModelState.IsValid) return View(vm);
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync($"{baseAddress}/termsofservice/update", stringContent);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return View(vm);
	}

	public async Task<IActionResult> CreateFAQ()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> CreateFAQ(CreateFAQViewModel vm)
	{
		if (!ModelState.IsValid) return View();
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/faqs/create", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("faqs");
		}
		return View();
	}
	[HttpGet]
	public async Task<IActionResult> UpdateFAQ(int id)
	{
		var response = await _httpClient.GetAsync($"{baseAddress}/faqs/getbyid/{id}");
		if (!response.IsSuccessStatusCode)
		{
			return RedirectToAction("index");
		}

		var dataStr = await response.Content.ReadAsStringAsync();
		var vm = JsonConvert.DeserializeObject<UpdateFAQViewModel>(dataStr);

		return View(vm);
	}

	[HttpPost]
	public async Task<IActionResult> UpdateFAQ(int id, UpdateFAQViewModel vm)
	{
		if (!ModelState.IsValid) return View(vm);
		vm.Id = id;
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync($"{baseAddress}/faqs/update/{id}", stringContent);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("faqs");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
		return View(vm);
	}

	

	public async Task<IActionResult> FAQs(int itemPerPage = 5, int page = 1)
	{
		var response = await _httpClient.GetAsync(baseAddress + "/faqs/getall");
		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();
			var act = JsonConvert.DeserializeObject<List<FAQGetViewModel>>(responseData);
			var queryableItems = act.AsQueryable();
			var paginatedDatas = PaginatedList<FAQGetViewModel>.Create(queryableItems, itemPerPage, page);

			return View(paginatedDatas);
		}
		return View();
	}

    //[HttpGet]
    //public async Task<IActionResult> ReplyMessage(int id)
    //{
    //    var response = await _httpClient.GetAsync($"{baseAddress}/messages/getbyid/{id}");
    //    if (!response.IsSuccessStatusCode)
    //    {
    //        return RedirectToAction("index");
    //    }

    //    var dataStr = await response.Content.ReadAsStringAsync();
    //    var vm = JsonConvert.DeserializeObject<MessageGetAllViewModel>(dataStr);

    //    return View(vm);
    //}
    public IActionResult ReplyMessage(int id,string text)
	{
		ViewBag.Id = id;
		ViewBag.Text = text;
		return View();
	}

    [HttpPost]
    public async Task<IActionResult> ReplyMessage(int id, MessageReplyViewModel vm)
    {
        vm.Id = id;
        //if (!ModelState.IsValid) return View(vm);
        var dataStr = JsonConvert.SerializeObject(vm);
        var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{baseAddress}/messages/reply/{id}", stringContent);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("messages");
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Console.WriteLine(responseContent);
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Messages(int itemPerPage = 5, int page = 1)
    
	{
        var response = await _httpClient.GetAsync(baseAddress + "/messages/getall");
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var act = JsonConvert.DeserializeObject<List<MessageGetAllViewModel>>(responseData);
            var queryableItems = act.AsQueryable();
            var paginatedDatas = PaginatedList<MessageGetAllViewModel>.Create(queryableItems, itemPerPage, page);
			MessagePageViewModel vm = new()
			{
				List = paginatedDatas,
				Reply = new MessageReplyViewModel()
			};
            return View(vm);
        }
        return View();
    }
	public async Task<IActionResult> DeleteMessage(int id)
	{
		if (!ModelState.IsValid) return RedirectToAction("Messages");
		var response = await _httpClient.DeleteAsync(baseAddress + $"/messages/delete/{id}");

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("messages");
		}
		return View();
	}
}
