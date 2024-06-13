using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class ResetPasswordViewModel
{
	public string Token { get; set; }
	[DataType(DataType.Password)]
	[Required]
	[MaxLength(50)]
	[StringLength(100, ErrorMessage = "The password must be at least 6 and at most 50 characters long.", MinimumLength = 6)]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Password must be at least 6 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one non-alphanumeric character.")]
	public string Password { get; set; }
	[Compare("Password")]
	[Required]
	public string ConfirmPassword { get; set; }
}
