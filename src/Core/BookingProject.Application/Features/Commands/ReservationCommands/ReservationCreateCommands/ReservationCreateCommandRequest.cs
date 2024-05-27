using MediatR;

namespace BookingProject.Application.Features.Commands.ReservationCommands.ReservationCreateCommands;

public class ReservationCreateCommandRequest:IRequest<ReservationCreateCommandResponse>
{
	public required int RoomId { get; set; }
	public bool IsPaid { get; set; }
	public required DateTime StartTime { get; set; }
	public required DateTime EndTime { get; set; }
}
