using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class LoginViewModel
{
	[DataType(DataType.Text)]
	[Required]
	[MaxLength(256)]
	public string UserName { get; set; }
	[DataType(DataType.Password)]
	[Required]
	[MaxLength(50)]
	public string Password { get; set; }
}
