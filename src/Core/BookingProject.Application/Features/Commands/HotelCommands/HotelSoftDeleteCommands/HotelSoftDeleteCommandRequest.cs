using MediatR;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelSoftDeleteCommands;

public class HotelSoftDeleteCommandRequest:IRequest<HotelSoftDeleteCommandResponse>
{
    public required int Id { get; set; }
}
