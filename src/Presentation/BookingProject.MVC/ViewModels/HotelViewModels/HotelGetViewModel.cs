using BookingProject.Application.Features.Queries.ReviewQueries;
using BookingProject.Application.Features.Queries.RoomQueries;
using BookingProject.MVC.ViewModels.RoomViewModels;

namespace BookingProject.MVC.ViewModels.HotelViewModels;

public class HotelGetViewModel
{
	public int Id { get; set; }
	public bool IsInWishlist { get; set; } = false;
	public int? WishlistItemId { get; set; } = null;

	public string Name { get; set; }
	public string Desc { get; set; }
	public string Address { get; set; }
	public string CountryName { get; set; }
	public string City { get; set; }
	public bool IsDeactive { get; set; }
	public bool IsApproved { get; set; }
	public bool IsRefused { get; set; }
	public string AppUserId { get; set; }
	public List<ReviewGetAllQueryResponse> Reviews { get; set; }
	public int ViewerCount { get; set; }
	public decimal StarPoint { get; set; }
	public string TypeName { get; set; }
	public List<string> ActivityNames { get; set; }
	public List<string> ImageFileUrls { get; set; }
	public List<string> AdvantageNames { get; set; }
	public List<string> PaymentMethodNames { get; set; }
	public List<string> ServiceNames { get; set; }
	public List<string> StaffLanguageNames { get; set; }
	public List<RoomGetViewModel> Rooms { get; set; }

}
