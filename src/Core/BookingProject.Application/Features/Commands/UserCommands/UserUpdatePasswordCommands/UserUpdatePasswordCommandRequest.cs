using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.UserCommands.UserUpdatePasswordCommands;

public class UserUpdatePasswordCommandRequest:IRequest<UserUpdatePasswordCommandResponse>
{
	[DataType(DataType.Password)]
    [Required]
    public string OldPassword { get; set; }
    [DataType(DataType.Password)]
    [MaxLength(50)]
    [Required]
    [StringLength(100, ErrorMessage = "The password must be at least 6 and at most 50 characters long.", MinimumLength = 6)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Password must be at least 6 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one non-alphanumeric character.")]
    public required string NewPassword { get; set; }
	[DataType(DataType.Password)]
	[MaxLength(100)]
    [Required]
    [Compare("NewPassword")]
	public string ConfirmNewPassword { get; set; }
	public string? Id { get; set; }
}
