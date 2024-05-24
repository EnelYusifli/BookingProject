using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.MVC.ViewModels.HotelViewModels;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class WishlistViewModel
{
	public int Id { get; set; }
	public int HotelId { get; set; }
	public HotelGetViewModel Hotel { get; set; }
}
