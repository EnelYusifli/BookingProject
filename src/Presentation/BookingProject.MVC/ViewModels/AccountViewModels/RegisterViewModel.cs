using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class RegisterViewModel
{
	[DataType(DataType.Text)]
	[MaxLength(256)]
	[Required]
	public string UserName { get; set; }
	[DataType(DataType.EmailAddress)]
	[MaxLength(256)]
	[Required]
	public string Email { get; set; }
	[DataType(DataType.Text)]
	[MaxLength(100)]
	[Required]
	public string FirstName { get; set; }
	[DataType(DataType.Text)]
	[MaxLength(100)]
	[Required]
	public string LastName { get; set; }
	[DataType(DataType.Date)]
	[Required]
	public DateOnly Birthdate { get; set; }
	[DataType(DataType.Password)]
	[MaxLength(50)]
	[Required]
	[StringLength(100, ErrorMessage = "The password must be at least 6 and at most 50 characters long.", MinimumLength = 6)]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Password must be at least 6 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one non-alphanumeric character.")]
	public string Password { get; set; }
	[DataType(DataType.Password)]
	[Compare("Password")]
	[MaxLength(50)]
	public string ConfirmPassword { get; set; }
}
