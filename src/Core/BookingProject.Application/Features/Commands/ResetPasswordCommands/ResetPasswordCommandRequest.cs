using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.ResetPasswordCommands;

public class ResetPasswordCommandRequest:IRequest<ResetPasswordCommandResponse>
{
	public required string Token { get; set; }
	public required string Password { get; set; }
	[Compare("Password")]
	public required string ConfirmPassword { get; set; }
}
