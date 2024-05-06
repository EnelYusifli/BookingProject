using MediatR;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelDeleteCommands;

public class HotelDeleteCommandRequest:IRequest<HotelDeleteCommandResponse>
{
    public required int Id { get; set; }
}
