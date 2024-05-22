namespace BookingProject.MVC.ViewModels.ProfileViewModels;

public class UpdatePersonalInfoViewModel
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string UserName { get; set; }
	public string PhoneNumber { get; set; }
	public DateOnly? Birthdate { get; set; }
	public IFormFile? ProfilePhoto { get; set; }
}
