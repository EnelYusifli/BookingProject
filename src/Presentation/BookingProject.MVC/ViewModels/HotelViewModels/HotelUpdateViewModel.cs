using BookingProject.Application.Features.DTOs;
using BookingProject.MVC.ViewModels.RoomViewModels;

namespace BookingProject.MVC.ViewModels.HotelViewModels;

public class HotelUpdateViewModel
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public int CountryId { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public List<IFormFile>? NewImageFiles { get; set; }
	public List<ImageDto>? Images { get; set; }
	public List<AdvantageDto>? Advantages { get; set; }
	public List<int>? StaffLanguageIds { get; set; }
    public List<int>? ServiceIds { get; set; }
    public List<int>? PaymentMethodIds { get; set; }
    public List<int>? ActivityIds { get; set; }
	public List<string>? HotelAdvantageNames { get; set; }
	public List<int>? DeletedAdvantageIds { get; set; }
	public List<int>? DeletedImageFileIds { get; set; }
	public List<RoomGetViewModel> Rooms { get; set; }
}
