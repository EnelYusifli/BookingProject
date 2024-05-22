using MediatR;

namespace BookingProject.Application.Features.Commands.UserCardCommands;

public class CardCreateCommandRequest:IRequest<CardCreateCommandResponse>
{
	public string CardNumber { get; set; }
	public string CVC { get; set; }
	public int ExpireMonth { get; set; }
	public int ExpireYear { get; set; }
}
