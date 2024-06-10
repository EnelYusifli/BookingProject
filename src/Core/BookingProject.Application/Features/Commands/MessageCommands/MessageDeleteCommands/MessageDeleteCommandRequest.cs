using MediatR;

namespace BookingProject.Application.Features.Commands.MessageCommands.MessageDeleteCommands;

public class MessageDeleteCommandRequest:IRequest<MessageDeleteCommandResponse>
{
    public required int Id { get; set; }
}
