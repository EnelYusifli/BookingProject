using BookingProject.Application.Features.DTOs;
using BookingProject.MVC.ViewModels.RoomViewModels;

namespace BookingProject.MVC.ViewModels.HotelViewModels;

public class HotelCreateViewModel
{
	public int TypeId { get; set; }
	public int CountryId { get; set; }
	public string AppUserId { get; set; }
	public string Name { get; set; }
	public string Desc { get; set; }
	public string Address { get; set; }
	public string City { get; set; }
	public List<RoomCreateViewModel> RoomCreateDtos { get; set; }
	public List<string>? HotelAdvantageNames { get; set; }
	public List<IFormFile> ImageFiles { get; set; }
	public List<int>? StaffLanguageIds { get; set; }
	public List<int>? ServiceIds { get; set; }
	public List<int>? PaymentMethodIds { get; set; }
	public List<int>? ActivityIds { get; set; }
}
