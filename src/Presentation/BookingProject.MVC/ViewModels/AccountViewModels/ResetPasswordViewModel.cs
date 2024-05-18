using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class ResetPasswordViewModel
{
	public string Token { get; set; }
	public string Password { get; set; }
	[Compare("Password")]
	public string ConfirmPassword { get; set; }
}
