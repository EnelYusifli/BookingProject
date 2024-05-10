using BookingProject.Application.CustomExceptions;
using MediatR;

namespace BookingProject.Application.Features.Commands.WishlistCommands.WishlistAddCommands;

public class WishlistAddCommandHandler : IRequestHandler<WishlistAddCommandRequest, WishlistAddCommandResponse>
{
	public async Task<WishlistAddCommandResponse> Handle(WishlistAddCommandRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			throw new NotFoundException("Request not found");

		return new WishlistAddCommandResponse();
	}
}
