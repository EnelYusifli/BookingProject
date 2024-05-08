using MediatR;

namespace BookingProject.Application.Features.Commands.RoomCommands.RoomDeleteCommands;

public class RoomDeleteCommandRequest:IRequest<RoomDeleteCommandResponse>
{
    public int Id { get; set; }
}
