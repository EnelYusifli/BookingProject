using MediatR;

namespace BookingProject.Application.Features.Commands.ReservationCommands.ReservationCreateCommands;

public class ReservationCreateCommandRequest:IRequest<ReservationCreateCommandResponse>
{
	public int RoomId { get; set; }
	public string AppUserId { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
}
