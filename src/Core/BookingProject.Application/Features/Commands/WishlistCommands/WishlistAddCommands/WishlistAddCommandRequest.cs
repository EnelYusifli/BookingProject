using MediatR;

namespace BookingProject.Application.Features.Commands.WishlistCommands.WishlistAddCommands;

public class WishlistAddCommandRequest:IRequest<WishlistAddCommandResponse>
{
    public required string UserId { get; set; }
    public required int HotelId { get; set; }
}
