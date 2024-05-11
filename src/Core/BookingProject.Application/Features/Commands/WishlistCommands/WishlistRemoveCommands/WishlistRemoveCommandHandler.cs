using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.WishlistCommands.WishlistRemoveCommands;

public class WishlistRemoveCommandHandler : IRequestHandler<WishlistRemoveCommandRequest, WishlistRemoveCommandResponse>
{
	private readonly IWishlistRepository _wishlistRepository;

	public WishlistRemoveCommandHandler(IWishlistRepository wishlistRepository)
    {
		_wishlistRepository = wishlistRepository;
	}
    public async Task<WishlistRemoveCommandResponse> Handle(WishlistRemoveCommandRequest request, CancellationToken cancellationToken)
	{
		UserWishlistHotel wishlistItem=await _wishlistRepository.GetByIdAsync(request.Id);
		if (wishlistItem is null || wishlistItem.IsDeactive==true)
			throw new NotFoundException("Hotel not found in wishlist");
		_wishlistRepository.Delete(wishlistItem);
		await _wishlistRepository.CommitAsync();
		return new WishlistRemoveCommandResponse();
	}
}
