using BookingProject.Application.Features.DTOs;
using MediatR;

namespace BookingProject.Application.Features.Commands.UserCommands.RefreshCommands;

public class RefreshCommandRequest:IRequest<RefreshCommandResponse>
{
	public TokenDto Token { get; set; }
}
