using Azure;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.MVC.Services;
using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Activity;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.ProfileViewModels;
using BookingProject.MVC.ViewModels.PropertyViewModels;
using BookingProject.MVC.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace BookingProject.MVC.Controllers;
[Authorize]
public class PropertyController : Controller
{
    Uri baseAddress = new Uri("https://localhost:7197/api");
    private readonly HttpClient _httpClient;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRoomRepository _roomRepository;

    public PropertyController(HttpClient httpClient,IRoomRepository roomRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _userManager = userManager;
		_roomRepository = roomRepository;
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
    public IActionResult JoinUs()
    {
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
        AppUser user = await GetCurrentUserAsync();
        hotelCreateViewModel.UserId = user.Id;

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
            content.Add(new StringContent(hotelCreateViewModel.UserId), nameof(hotelCreateViewModel.UserId));
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
    public async Task<IActionResult> UpdateHotel(int id)
    {
        PropertyViewModel viewModel = new PropertyViewModel();
        await PopulatePropertyViewModel(viewModel);
        ViewBag.Property = viewModel;
        //GetByIdForUpdate
        var response = await _httpClient.GetAsync($"{baseAddress}/hotels/getbyidforupdate/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var dataStr = await response.Content.ReadAsStringAsync();
        var vm = JsonConvert.DeserializeObject<HotelUpdateViewModel>(dataStr);

        return View(vm);

    }
    [HttpPost]
    public async Task<IActionResult> UpdateHotel(HotelUpdateViewModel vm)
    {
        //AppUser user = await GetCurrentUserAsync();
        //vm.UserId = user.Id;

        //if (!ModelState.IsValid)
        //{
        //    //var responseId2 = await _httpClient.GetAsync(baseAddress + "/acc/getauthuser");

        //    //if (responseId2.IsSuccessStatusCode)
        //    //{
        //    //	var responseData = await responseId2.Content.ReadAsStringAsync();
        //    //	var dto = JsonConvert.DeserializeObject<UserViewModel>(responseData);
        //    //	ViewBag.UserId = dto.User.Id;
        //    //}
        //    PropertyViewModel vm2 = new PropertyViewModel();
        //    await PopulatePropertyViewModel(vm2);
        //    ViewBag.Property = vm2;
        //    return View(vm);
        //}
        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(vm.TypeId.ToString()), nameof(vm.TypeId));
            content.Add(new StringContent(vm.Id.ToString()), nameof(vm.Id));
            //content.Add(new StringContent(vm.UserId), nameof(vm.UserId));
            content.Add(new StringContent(vm.Name), nameof(vm.Name));
            content.Add(new StringContent(vm.Desc), nameof(vm.Desc));
            content.Add(new StringContent(vm.Address), nameof(vm.Address));
            content.Add(new StringContent(vm.CountryId.ToString()), nameof(vm.CountryId));
            content.Add(new StringContent(vm.City), nameof(vm.City));
            content.Add(new StringContent(vm.IsDeactive.ToString()), nameof(vm.IsDeactive));

            //if (vm.HotelAdvantageNames != null)
            //{
            //    foreach (var advantageName in vm.HotelAdvantageNames)
            //    {
            //        content.Add(new StringContent(advantageName), nameof(hotelCreateViewModel.HotelAdvantageNames));
            //    }
            //}

            if (vm.StaffLanguageIds != null)
            {
                foreach (var id in vm.StaffLanguageIds)
                {
                    content.Add(new StringContent(id.ToString()), nameof(vm.StaffLanguageIds));
                }
            }

            if (vm.ServiceIds != null)
            {
                foreach (var id in vm.ServiceIds)
                {
                    content.Add(new StringContent(id.ToString()), nameof(vm.ServiceIds));
                }
            }

            if (vm.PaymentMethodIds != null)
            {
                foreach (var id in vm.PaymentMethodIds)
                {
                    content.Add(new StringContent(id.ToString()), nameof(vm.PaymentMethodIds));
                }
            }

            if (vm.ActivityIds != null)
            {
                foreach (var id in vm.ActivityIds)
                {
                    content.Add(new StringContent(id.ToString()), nameof(vm.ActivityIds));
                }
            }
            if (vm.DeletedImageFileIds != null)
            {
                foreach (var id in vm.DeletedImageFileIds)
                {
                    content.Add(new StringContent(id.ToString()), nameof(vm.DeletedImageFileIds));
                }
            }

            if (vm.NewImageFiles != null)
            {
                foreach (var file in vm.NewImageFiles)
                {
                    if (file != null)
                    {
                        var fileContent = new StreamContent(file.OpenReadStream());
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                        content.Add(fileContent, nameof(vm.NewImageFiles), file.FileName);
                    }
                }
            }

            //if (vm.RoomCreateDtos != null)
            //{
            //    for (int i = 0; i < vm.RoomCreateDtos.Count; i++)
            //    {
            //        var room = vm.RoomCreateDtos[i];
            //        content.Add(new StringContent(room.RoomName), $"RoomCreateDtos[{i}].RoomName");
            //        content.Add(new StringContent(room.AdultCount.ToString()), $"RoomCreateDtos[{i}].AdultCount");
            //        content.Add(new StringContent(room.ChildCount.ToString()), $"RoomCreateDtos[{i}].ChildCount");
            //        content.Add(new StringContent(room.ServiceFee.ToString()), $"RoomCreateDtos[{i}].ServiceFee");
            //        content.Add(new StringContent(room.PricePerNight.ToString()), $"RoomCreateDtos[{i}].PricePerNight");
            //        content.Add(new StringContent(room.Area.ToString()), $"RoomCreateDtos[{i}].Area");
            //        content.Add(new StringContent(room.IsCancellable.ToString()), $"RoomCreateDtos[{i}].IsCancellable");

            //        if (room.CancelAfterDay.HasValue)
            //        {
            //            content.Add(new StringContent(room.CancelAfterDay.Value.ToString()), $"RoomCreateDtos[{i}].CancelAfterDay");
            //        }

            //        if (room.ImageFiles != null)
            //        {
            //            foreach (var file in room.ImageFiles)
            //            {
            //                if (file != null)
            //                {
            //                    var fileContent = new StreamContent(file.OpenReadStream());
            //                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
            //                    content.Add(fileContent, $"RoomCreateDtos[{i}].ImageFiles", file.FileName);
            //                }
            //            }
            //        }
            //    }
            //}

            var response = await _httpClient.PutAsync(baseAddress + $"/hotels/update/{vm.Id}", content);

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
        PropertyViewModel vm3 = new PropertyViewModel();
        await PopulatePropertyViewModel(vm3);
        ViewBag.Property = vm3;
        return View(vm);
    }
    public IActionResult AddRoom(int hotelid)
    {
        ViewBag.HotelId = hotelid;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddRoom(RoomCreateViewModel vm,int hotelid)
    {
        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(vm.RoomName), nameof(vm.RoomName));
            content.Add(new StringContent(vm.AdultCount.ToString()), nameof(vm.AdultCount));
            if (vm.HotelId.HasValue)
            {
                content.Add(new StringContent(vm.HotelId.ToString()), nameof(vm.HotelId));
            }
            content.Add(new StringContent(vm.ChildCount.ToString()), nameof(vm.ChildCount));
            content.Add(new StringContent(vm.ServiceFee.ToString()), nameof(vm.ServiceFee));
            content.Add(new StringContent(vm.PricePerNight.ToString()), nameof(vm.PricePerNight));
            content.Add(new StringContent(vm.Area.ToString()), nameof(vm.Area));
            content.Add(new StringContent(vm.IsCancellable.ToString()), nameof(vm.IsCancellable));

            if (vm.CancelAfterDay.HasValue)
            {
                content.Add(new StringContent(vm.CancelAfterDay.Value.ToString()), nameof(vm.CancelAfterDay));
            }
            else
            {
                vm.CancelAfterDay = 0;
				content.Add(new StringContent(vm.CancelAfterDay.Value.ToString()), nameof(vm.CancelAfterDay));
			}
            if (vm.ImageFiles != null)
            {
                foreach (var file in vm.ImageFiles)
                {
                    if (file != null)
                    {
                        var fileContent = new StreamContent(file.OpenReadStream());
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                        content.Add(fileContent, nameof(vm.ImageFiles), file.FileName);
						Console.WriteLine($"File: {file.FileName}, Size: {file.Length}");
					}
                }
            }
		var response = await _httpClient.PostAsync(baseAddress + $"/rooms/create", content);

		if (response.IsSuccessStatusCode)
		{
            await _roomRepository.CommitAsync();
			return RedirectToAction("HotelAdded");
		}
		else
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(responseContent);
		}
        }
		ViewBag.HotelId = hotelid;
		return View();
	}
    public async Task<IActionResult> UpdateRoom(int id)
    {
		var response = await _httpClient.GetAsync($"{baseAddress}/rooms/getbyid/{id}");
		if (!response.IsSuccessStatusCode)
		{
			return NotFound();
		}

		var dataStr = await response.Content.ReadAsStringAsync();
		var vm = JsonConvert.DeserializeObject<RoomUpdateViewModel>(dataStr);

		return View(vm);
	}
    [HttpPost]
    public async Task<IActionResult> UpdateRoom(RoomUpdateViewModel vm) {
		using (var content = new MultipartFormDataContent())
		{
			content.Add(new StringContent(vm.RoomName), nameof(vm.RoomName));
			content.Add(new StringContent(vm.AdultCount.ToString()), nameof(vm.AdultCount));
			content.Add(new StringContent(vm.ChildCount.ToString()), nameof(vm.ChildCount));
			content.Add(new StringContent(vm.ServiceFee.ToString()), nameof(vm.ServiceFee));
			content.Add(new StringContent(vm.PricePerNight.ToString()), nameof(vm.PricePerNight));
			content.Add(new StringContent(vm.Area.ToString()), nameof(vm.Area));
			content.Add(new StringContent(vm.IsCancellable.ToString()), nameof(vm.IsCancellable));

			if (vm.CancelAfterDay.HasValue)
			{
				content.Add(new StringContent(vm.CancelAfterDay.Value.ToString()), nameof(vm.CancelAfterDay));
			}
			else
			{
				vm.CancelAfterDay = 0;
				content.Add(new StringContent(vm.CancelAfterDay.Value.ToString()), nameof(vm.CancelAfterDay));
			}
			if (vm.ImageFiles != null)
			{
				foreach (var file in vm.ImageFiles)
				{
					if (file != null)
					{
						var fileContent = new StreamContent(file.OpenReadStream());
						fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
						content.Add(fileContent, nameof(vm.ImageFiles), file.FileName);
						Console.WriteLine($"File: {file.FileName}, Size: {file.Length}");
					}
				}
			}
			var response = await _httpClient.PutAsync(baseAddress + $"/rooms/update/{vm.Id}", content);

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
		return View();
	}

}

