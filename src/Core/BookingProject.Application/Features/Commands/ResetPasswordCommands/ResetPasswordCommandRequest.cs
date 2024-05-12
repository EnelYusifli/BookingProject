using MediatR;

namespace BookingProject.Application.Features.Commands.ResetPasswordCommands;

public class ResetPasswordCommandRequest:IRequest<ResetPasswordCommandResponse>
{
	public string Token { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
}
