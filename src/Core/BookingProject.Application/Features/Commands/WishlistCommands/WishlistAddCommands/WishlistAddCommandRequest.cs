using MediatR;

namespace BookingProject.Application.Features.Commands.WishlistCommands.WishlistAddCommands;

public class WishlistAddCommandRequest:IRequest<WishlistAddCommandResponse>
{
    public string UserId { get; set; }
    public int HotelId { get; set; }
}
