using Azure;
using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.ProfileViewModels;
using BookingProject.MVC.ViewModels.PropertyViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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
		PropertyViewModel viewModel = new PropertyViewModel();
		//var response = await _httpClient.GetAsync(baseAddress + "/acc/getauthuser");

		//if (response.IsSuccessStatusCode)
		//{
		//	var responseData = await response.Content.ReadAsStringAsync();
		//	var dto = JsonConvert.DeserializeObject<UserViewModel>(responseData);
  //          ViewBag.UserId=dto.User.Id;
		//}
		if (!ModelState.IsValid) return View(viewModel);
		await PopulatePropertyViewModel(viewModel);
		ViewBag.Property = viewModel;

		return View();
	}


	[HttpPost]
    public async Task<IActionResult> AddHotel(HotelCreateViewModel hotelCreateViewModel)
    {
		if (!ModelState.IsValid)
		{
			//var responseId2 = await _httpClient.GetAsync(baseAddress + "/acc/getauthuser");

			//if (responseId2.IsSuccessStatusCode)
			//{
			//	var responseData = await responseId2.Content.ReadAsStringAsync();
			//	var dto = JsonConvert.DeserializeObject<UserViewModel>(responseData);
			//	ViewBag.UserId = dto.User.Id;
			//}
			PropertyViewModel vm2 = new PropertyViewModel();
			await PopulatePropertyViewModel(vm2);
			ViewBag.Property = vm2;
			return View();
		}
        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(hotelCreateViewModel.TypeId.ToString()), nameof(hotelCreateViewModel.TypeId));
            content.Add(new StringContent(hotelCreateViewModel.Name), nameof(hotelCreateViewModel.Name));
            content.Add(new StringContent(hotelCreateViewModel.Desc), nameof(hotelCreateViewModel.Desc));
            content.Add(new StringContent(hotelCreateViewModel.Address), nameof(hotelCreateViewModel.Address));
            content.Add(new StringContent(hotelCreateViewModel.CountryId.ToString()), nameof(hotelCreateViewModel.CountryId));
            content.Add(new StringContent(hotelCreateViewModel.City), nameof(hotelCreateViewModel.City));

            if (hotelCreateViewModel.HotelAdvantageNames != null)
            {
                foreach (var advantageName in hotelCreateViewModel.HotelAdvantageNames)
                {
                    content.Add(new StringContent(advantageName), nameof(hotelCreateViewModel.HotelAdvantageNames));
                }
            }

            if (hotelCreateViewModel.StaffLanguageIds != null)
            {
                foreach (var id in hotelCreateViewModel.StaffLanguageIds)
                {
                    content.Add(new StringContent(id.ToString()), nameof(hotelCreateViewModel.StaffLanguageIds));
                }
            }

            if (hotelCreateViewModel.ServiceIds != null)
            {
                foreach (var id in hotelCreateViewModel.ServiceIds)
                {
                    content.Add(new StringContent(id.ToString()), nameof(hotelCreateViewModel.ServiceIds));
                }
            }

            if (hotelCreateViewModel.PaymentMethodIds != null)
            {
                foreach (var id in hotelCreateViewModel.PaymentMethodIds)
                {
                    content.Add(new StringContent(id.ToString()), nameof(hotelCreateViewModel.PaymentMethodIds));
                }
            }

            if (hotelCreateViewModel.ActivityIds != null)
            {
                foreach (var id in hotelCreateViewModel.ActivityIds)
                {
                    content.Add(new StringContent(id.ToString()), nameof(hotelCreateViewModel.ActivityIds));
                }
            }

            if (hotelCreateViewModel.ImageFiles != null)
            {
                foreach (var file in hotelCreateViewModel.ImageFiles)
                {
                    if (file != null)
                    {
                        var fileContent = new StreamContent(file.OpenReadStream());
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                        content.Add(fileContent, nameof(hotelCreateViewModel.ImageFiles), file.FileName);
                    }
                }
            }

            if (hotelCreateViewModel.RoomCreateDtos != null)
            {
                for (int i = 0; i < hotelCreateViewModel.RoomCreateDtos.Count; i++)
                {
                    var room = hotelCreateViewModel.RoomCreateDtos[i];
                    content.Add(new StringContent(room.RoomName), $"RoomCreateDtos[{i}].RoomName");
                    content.Add(new StringContent(room.AdultCount.ToString()), $"RoomCreateDtos[{i}].AdultCount");
                    content.Add(new StringContent(room.ChildCount.ToString()), $"RoomCreateDtos[{i}].ChildCount");
                    content.Add(new StringContent(room.ServiceFee.ToString()), $"RoomCreateDtos[{i}].ServiceFee");
                    content.Add(new StringContent(room.PricePerNight.ToString()), $"RoomCreateDtos[{i}].PricePerNight");
                    content.Add(new StringContent(room.Area.ToString()), $"RoomCreateDtos[{i}].Area");
                    content.Add(new StringContent(room.IsCancellable.ToString()), $"RoomCreateDtos[{i}].IsCancellable");

                    if (room.CancelAfterDay.HasValue)
                    {
                        content.Add(new StringContent(room.CancelAfterDay.Value.ToString()), $"RoomCreateDtos[{i}].CancelAfterDay");
                    }

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
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                Console.WriteLine(responseContent);
            }
        }
		//var responseId = await _httpClient.GetAsync(baseAddress + "/acc/getauthuser");

		//if (responseId.IsSuccessStatusCode)
		//{
		//	var responseData = await responseId.Content.ReadAsStringAsync();
		//	var dto = JsonConvert.DeserializeObject<UserViewModel>(responseData);
		//	ViewBag.UserId = dto.User.Id;
		//}
		PropertyViewModel vm = new PropertyViewModel();
        await PopulatePropertyViewModel(vm);
        ViewBag.Property = vm;
        return View();
    }

    public IActionResult HotelAdded()
	{
		return View();
	}
}
