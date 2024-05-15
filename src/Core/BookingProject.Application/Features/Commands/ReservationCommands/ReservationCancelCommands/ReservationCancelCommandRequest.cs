using MediatR;

namespace BookingProject.Application.Features.Commands.ReservationCommands.ReservationCancelCommands;

public class ReservationCancelCommandRequest:IRequest<ReservationCancelCommandResponse>
{
    public int ReservationId { get; set; }
}
