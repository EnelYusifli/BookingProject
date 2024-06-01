using MediatR;

namespace BookingProject.Application.Features.Commands.WishlistCommands.WishlistAddCommands;

public class WishlistAddCommandRequest:IRequest<WishlistAddCommandResponse>
{
    public int HotelId { get; set; }
    public string Id { get; set; }
}
