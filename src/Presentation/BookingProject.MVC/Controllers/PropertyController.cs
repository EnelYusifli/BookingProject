using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.PropertyViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
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

		var activitiesResponse = await _httpClient.GetAsync(baseAddress + "/activities/getall");
		if (activitiesResponse.IsSuccessStatusCode)
		{
			var activitiesData = await activitiesResponse.Content.ReadAsStringAsync();
			propertyViewModel.Activities = JsonConvert.DeserializeObject<List<ActivityGetViewModel>>(activitiesData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve activities data.");
		}

		var paymentMethodsResponse = await _httpClient.GetAsync(baseAddress + "/paymentmethods/getall");
		if (paymentMethodsResponse.IsSuccessStatusCode)
		{
			var paymentMethodsData = await paymentMethodsResponse.Content.ReadAsStringAsync();
			propertyViewModel.PaymentMethods = JsonConvert.DeserializeObject<List<PaymentMethodGetViewModel>>(paymentMethodsData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve payment methods data.");
		}

		var staffLanguagesResponse = await _httpClient.GetAsync(baseAddress + "/stafflanguages/getall");
		if (staffLanguagesResponse.IsSuccessStatusCode)
		{
			var staffLanguagesData = await staffLanguagesResponse.Content.ReadAsStringAsync();
			propertyViewModel.StaffLanguages = JsonConvert.DeserializeObject<List<StaffLanguageGetViewModel>>(staffLanguagesData);
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Failed to retrieve staff languages data.");
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

	public async Task<IActionResult> AddHotel()
	{
		//AddHotelViewModel viewModel = new AddHotelViewModel();
		PropertyViewModel viewModel = new PropertyViewModel();

		if (!ModelState.IsValid) return View(viewModel);

		await PopulatePropertyViewModel(viewModel);
		ViewBag.Property = viewModel;

		return View();
	}


	[HttpPost]
	public async Task<IActionResult> AddHotel(HotelCreateViewModel hotelCreateViewModel)
	{
		//if (!ModelState.IsValid)
		//{
		//	if (vm.PropertyViewModel == null)
		//	{
		//		vm.PropertyViewModel = new PropertyViewModel();
		//	}

		//	await PopulatePropertyViewModel(vm.PropertyViewModel);
		//	return View(vm);
		//}

		//HotelCreateViewModel hotelCreateViewModel = vm.HotelCreateViewModel;

		using (var content = new MultipartFormDataContent())
		{
			var dataStr = JsonConvert.SerializeObject(hotelCreateViewModel);
			var stringContent = new StringContent(dataStr, Encoding.UTF8, "application/json");
			content.Add(stringContent, "hotelCreateViewModel");

			if (hotelCreateViewModel.ImageFiles != null)
			{
				foreach (var file in hotelCreateViewModel.ImageFiles)
				{
					if (file != null)
					{
						var fileContent = new StreamContent(file.OpenReadStream());
						fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
						content.Add(fileContent, "HotelImageFiles", file.FileName);
					}
				}
			}

			if (hotelCreateViewModel.RoomCreateDtos != null)
			{
				for (int i = 0; i < hotelCreateViewModel.RoomCreateDtos.Count; i++)
				{
					var room = hotelCreateViewModel.RoomCreateDtos[i];
					if (room.ImageFiles != null)
					{
						foreach (var file in room.ImageFiles)
						{
							if (file != null)
							{
								var fileContent = new StreamContent(file.OpenReadStream());
								fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
								content.Add(fileContent, $"RoomCreateDtos[{i}].ImageFiles", file.FileName);
							}
						}
					}
				}
			}
			var response = await _httpClient.PostAsync(baseAddress + "/hotels/create", content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("HotelAdded");
			}
		}
			PropertyViewModel vm = new PropertyViewModel();
			await PopulatePropertyViewModel(vm);
		    ViewBag.Property= vm;
			return View();
	}



	public IActionResult HotelAdded()
	{
		return View();
	}
}
