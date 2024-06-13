using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class ForgotPasswordViewModel
{
	[DataType(DataType.EmailAddress)]
	[MaxLength(100)]
	public string Email { get; set; }
}
