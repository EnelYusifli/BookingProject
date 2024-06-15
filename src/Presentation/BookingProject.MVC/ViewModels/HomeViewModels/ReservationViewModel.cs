using BookingProject.MVC.ViewModels.AccountViewModels;
using BookingProject.MVC.ViewModels.HotelViewModels;
using BookingProject.MVC.ViewModels.RoomViewModels;

namespace BookingProject.MVC.ViewModels.HomeViewModels;

public class ReservationViewModel
{
	public HotelGetViewModel Hotel { get; set; }
	public RoomGetViewModel Room { get; set; }
	public string CheckInDate { get; set; }
	public string AdultCount { get; set; } = "1";
	public string ChildCount { get; set; } = "0";
	public string CheckOutDate { get; set; }
	public int Nights { get; set; }


}
