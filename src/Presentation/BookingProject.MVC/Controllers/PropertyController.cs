using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.PropertyViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace BookingProject.MVC.Controllers;
public class PropertyController : Controller
{
	Uri baseAddress = new Uri("https://localhost:7197/api");
	private readonly HttpClient _httpClient;

	public PropertyController(HttpClient httpClient)
	{
		_httpClient = httpClient;
		_httpClient.BaseAddress = baseAddress;
	}
	public IActionResult JoinUs()
	{
		return View();
	}
	public async Task<IActionResult> ApproveHotel([FromRoute]int id)
	{
		ApproveHotelViewModel vm = new()
		{
			Id = id
		};
		if (!ModelState.IsValid) return View();
		var dataStr = JsonConvert.SerializeObject(vm);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + $"/hotel/approvehotel/{id}", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Index", "Home");
		}
		return View();
	}
	public async Task<IActionResult> AddHotel()
	{
		AddHotelViewModel viewModel = new AddHotelViewModel();
		viewModel.PropertyViewModel = new PropertyViewModel();
		if (!ModelState.IsValid) return View(viewModel);

		var typesResponse = await _httpClient.GetAsync(baseAddress + "/types/getall");
		if (typesResponse.IsSuccessStatusCode)
		{
			var typesData = await typesResponse.Content.ReadAsStringAsync();
			viewModel.PropertyViewModel.Types = JsonConvert.DeserializeObject<List<TypeGetViewModel>>(typesData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve types data.");
		}

		var activitiesResponse = await _httpClient.GetAsync(baseAddress + "/activities/getall");
		if (activitiesResponse.IsSuccessStatusCode)
		{
			var activitiesData = await activitiesResponse.Content.ReadAsStringAsync();
			viewModel.PropertyViewModel.Activities = JsonConvert.DeserializeObject<List<ActivityGetViewModel>>(activitiesData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve activities data.");
		}

		var paymentMethodsResponse = await _httpClient.GetAsync(baseAddress + "/paymentmethods/getall");
		if (paymentMethodsResponse.IsSuccessStatusCode)
		{
			var paymentMethodsData = await paymentMethodsResponse.Content.ReadAsStringAsync();
			viewModel.PropertyViewModel.PaymentMethods = JsonConvert.DeserializeObject<List<PaymentMethodGetViewModel>>(paymentMethodsData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve payment methods data.");
		}

		var staffLanguagesResponse = await _httpClient.GetAsync(baseAddress + "/stafflanguages/getall");
		if (staffLanguagesResponse.IsSuccessStatusCode)
		{
			var staffLanguagesData = await staffLanguagesResponse.Content.ReadAsStringAsync();
			viewModel.PropertyViewModel.StaffLanguages = JsonConvert.DeserializeObject<List<StaffLanguageGetViewModel>>(staffLanguagesData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve staff languages data.");
		}

		var servicesResponse = await _httpClient.GetAsync(baseAddress + "/services/getall");
		if (servicesResponse.IsSuccessStatusCode)
		{
			var servicesData = await servicesResponse.Content.ReadAsStringAsync();
			viewModel.PropertyViewModel.Services = JsonConvert.DeserializeObject<List<ServiceGetViewModel>>(servicesData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve services data.");
		}

		viewModel.HotelCreateViewModel = new HotelCreateViewModel();

		return View(viewModel);
	}


	[HttpPost]
	public async Task<IActionResult> AddHotel(AddHotelViewModel vm)
	{
		//PropertyViewModel propertyViewModel = vm.PropertyViewModel;
		if (!ModelState.IsValid) return View(vm);
		HotelCreateViewModel hotelCreateViewModel = vm.HotelCreateViewModel;
		var dataStr = JsonConvert.SerializeObject(hotelCreateViewModel);
		var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(baseAddress + "/hotels/create", stringContent);


		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("HotelAdded");
		}
		return View(vm);
	}
	public IActionResult HotelAdded()
	{
		return View();
	}
}
