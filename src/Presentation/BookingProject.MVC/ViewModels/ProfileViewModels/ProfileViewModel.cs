using BookingProject.MVC.ViewModels.AccountViewModels;

namespace BookingProject.MVC.ViewModels.ProfileViewModels;

public class ProfileViewModel
{
	public UserViewModel User { get; set; }
	public UpdatePersonalInfoViewModel PersonalInfo{ get; set; } 
	public UpdatePasswordViewModel Password{ get; set; } 
}
