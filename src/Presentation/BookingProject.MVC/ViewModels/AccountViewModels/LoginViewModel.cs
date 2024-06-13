using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class LoginViewModel
{
	[DataType(DataType.Text)]
	[Required]
	public string UserName { get; set; }
	[DataType(DataType.Password)]
	[Required]
	public string Password { get; set; }
}
