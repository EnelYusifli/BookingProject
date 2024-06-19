using BookingProject.Application.Features.DTOs;

namespace BookingProject.MVC.ViewModels.RoomViewModels;

public class RoomUpdateViewModel
{
	public string RoomName { get; set; }
	public List<ImageDto> Images { get; set; }
	public int Id { get; set; }
	public bool IsDeactive { get; set; }
	public int AdultCount { get; set; }
	public int ChildCount { get; set; }
	public bool IsDepositNeeded { get; set; }
	public decimal ServiceFee { get; set; }
	public decimal PricePerNight { get; set; }
	public decimal Area { get; set; }
	public bool IsCancellable { get; set; }
	public int? CancelAfterDay { get; set; }
	public List<IFormFile>? ImageFiles { get; set; }
	public List<int>? DeletedImageFileIds { get; set; }
}
