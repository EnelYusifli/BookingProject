using MediatR;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelRefuseCommands;

public class HotelRefuseCommandRequest:IRequest<HotelRefuseCommandResponse>
{
    public int Id { get; set; }
}
