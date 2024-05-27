using MediatR;

namespace BookingProject.Application.Features.Commands.DiscountCommands.DiscountCreateCommands;

public class DiscountCreateCommandRequest:IRequest<DiscountCreateCommandResponse>
{
	public int RoomId { get; set; }
	public int Percent { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
}
