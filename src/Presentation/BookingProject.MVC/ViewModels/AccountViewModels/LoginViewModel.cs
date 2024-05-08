using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class LoginViewModel
{
	[DataType(DataType.Text)]
	public string UserName { get; set; }
	[DataType(DataType.Password)]
	public string Password { get; set; }
}
