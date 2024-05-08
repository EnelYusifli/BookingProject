using BookingProject.MVC.ViewModels.HotelViewModels;

namespace BookingProject.MVC.ViewModels.PropertyViewModels;

public class AddHotelViewModel
{
	public PropertyViewModel PropertyViewModel { get; set; }
	public HotelCreateViewModel HotelCreateViewModel { get; set; }
}
