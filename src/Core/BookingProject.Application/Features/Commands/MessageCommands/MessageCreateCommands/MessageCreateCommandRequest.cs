using MediatR;

namespace BookingProject.Application.Features.Commands.MessageCommands.MessageCreateCommands;

public class MessageCreateCommandRequest:IRequest<MessageCreateCommandResponse>
{
	public string Name { get; set; }
	public string Email { get; set; }
	public string MessageText { get; set; }
}
