using MediatR;

namespace BookingProject.Application.Features.Commands.WishlistCommands.WishlistRemoveCommands;

public class WishlistRemoveCommandRequest:IRequest<WishlistRemoveCommandResponse>
{
    public required int Id { get; set; }
}
