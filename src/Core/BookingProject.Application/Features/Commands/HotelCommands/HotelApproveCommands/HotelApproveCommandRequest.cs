using MediatR;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelApproveCommands;

public class HotelApproveCommandRequest:IRequest<HotelApproveCommandResponse>
{
    public int Id { get; set; }
}
