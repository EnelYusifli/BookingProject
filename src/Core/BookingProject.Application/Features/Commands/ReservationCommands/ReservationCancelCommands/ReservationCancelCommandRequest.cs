using MediatR;

namespace BookingProject.Application.Features.Commands.ReservationCommands.ReservationCancelCommands;

public class ReservationCancelCommandRequest:IRequest<ReservationCancelCommandResponse>
{
    public required int ReservationId { get; set; }
}
