using MediatR;

namespace BookingProject.Application.Features.Commands.WishlistCommands.WishlistAddCommands;

public class WishlistAddCommandRequest:IRequest<WishlistAddCommandResponse>
{
    public required int HotelId { get; set; }
}
