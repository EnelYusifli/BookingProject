using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.MVC.ViewModels.HotelViewModels;

namespace BookingProject.MVC.ViewModels.HomeViewModels;

public class HomeViewModel
{
	public ICollection<HotelGetViewModel> Hotels { get; set; }
}
