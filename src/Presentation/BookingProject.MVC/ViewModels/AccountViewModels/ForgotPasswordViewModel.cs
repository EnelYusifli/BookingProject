using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class ForgotPasswordViewModel
{
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; }
}
