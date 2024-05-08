using MediatR;

namespace BookingProject.Application.Features.Commands.RoomCommands.RoomSoftDeleteCommands;

public class RoomSoftDeleteCommandRequest:IRequest<RoomSoftDeleteCommandResponse>
{
    public int Id { get; set; }
}
