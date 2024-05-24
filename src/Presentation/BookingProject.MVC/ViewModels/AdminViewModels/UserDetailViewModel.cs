using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.MVC.ViewModels.HotelViewModels;

namespace BookingProject.MVC.ViewModels.AdminViewModels;

public class UserDetailViewModel
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public IList<string> Roles { get; set; }
	public string UserName { get; set; }
	public DateOnly? Birthdate { get; set; }
	public string? RecoveryEmail { get; set; }
	public string? ProfilePhotoUrl { get; set; }
	public List<HotelGetViewModel> Hotels { get; set; }
}
