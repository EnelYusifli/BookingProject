using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.ProfileViewModels;

public class UpdatePasswordViewModel
{
	public string? AppUserId { get; set; }
	[DataType(DataType.Password)]
	public string? OldPassword { get; set; }
	[DataType(DataType.Password)]
	public string? NewPassword { get; set; }
	[DataType(DataType.Password)]
	[Compare("NewPassword")]
	public string? ConfirmNewPassword { get; set; }
}
