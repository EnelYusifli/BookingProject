using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.WishlistCommands.WishlistAddCommands;

public class WishlistAddCommandHandler : IRequestHandler<WishlistAddCommandRequest, WishlistAddCommandResponse>
{
	private readonly IWishlistRepository _repository;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IHotelRepository _hotelRepository;
	private readonly UserManager<AppUser> _userManager;

	public WishlistAddCommandHandler(IWishlistRepository repository,IHttpContextAccessor httpContextAccessor,IHotelRepository hotelRepository,UserManager<AppUser> userManager)
    {
		_repository = repository;
		_httpContextAccessor = httpContextAccessor;
		_hotelRepository = hotelRepository;
		_userManager = userManager;
	}
    public async Task<WishlistAddCommandResponse> Handle(WishlistAddCommandRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			throw new NotFoundException("Request not found");
		Hotel hotel=await _hotelRepository.GetByIdAsync(request.HotelId);
		if (hotel is null || hotel.IsDeactive == true)
			throw new NotFoundException("Hotel not found");
		AppUser user = await _userManager.FindByIdAsync(request.Id);
		if (user is null)
			throw new NotFoundException("User not found");
		if (await _repository.Table.Where(x => x.HotelId == request.HotelId && x.UserId == user.Id).AnyAsync())
			throw new BadRequestException("Hotel is already in wishlist");
		UserWishlistHotel userWishlistHotel = new()
		{
			HotelId = request.HotelId,
			UserId = user.Id
		};
		await _repository.CreateAsync(userWishlistHotel);
		await _repository.CommitAsync();

		return new WishlistAddCommandResponse();
	}
}
