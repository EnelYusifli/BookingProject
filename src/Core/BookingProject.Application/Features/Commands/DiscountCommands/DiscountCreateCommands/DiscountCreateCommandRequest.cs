using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.DiscountCommands.DiscountCreateCommands;

public class DiscountCreateCommandRequest:IRequest<DiscountCreateCommandResponse>
{
	public int RoomId { get; set; }

	[Range(1, 100, ErrorMessage = "Percent must be between 1 and 100.")]
	public int Percent { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }
}
