using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class RegisterViewModel
{
	public string UserName { get; set; }
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; }
	[DataType(DataType.Text)]
	public string FirstName { get; set; }
	[DataType(DataType.Text)]
	public string LastName { get; set; }
	[DataType(DataType.Date)]
	public DateOnly Birthdate { get; set; }
	[DataType(DataType.Password)]
	public string Password { get; set; }
	[DataType(DataType.Password)]
	[Compare("Password")]
	public string ConfirmPassword { get; set; }
}
