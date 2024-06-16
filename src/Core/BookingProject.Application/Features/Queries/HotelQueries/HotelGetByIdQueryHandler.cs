using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetByIdQueryHandler : IRequestHandler<HotelGetByIdQueryRequest, HotelGetByIdQueryResponse>
{
	private readonly IHotelRepository _repository;
	private readonly IMapper _mapper;
	private readonly UserManager<AppUser> _userManager;

	public HotelGetByIdQueryHandler(IHotelRepository repository, IMapper mapper,UserManager<AppUser> userManager)
	{
		_repository = repository;
		_mapper = mapper;
		_userManager = userManager;
	}
	public async Task<HotelGetByIdQueryResponse> Handle(HotelGetByIdQueryRequest request, CancellationToken cancellationToken)
	{
		Hotel hotel = await _repository.Table
			.Include(x => x.HotelImages)
			.Include(x => x.CustomerReviews)
			.ThenInclude(x => x.User)
			.Include(x => x.CustomerReviews)
			.ThenInclude(x => x.ReviewImages)
			.Include(x => x.HotelAdvantages)
			.Include(x => x.HotelActivities)
			.ThenInclude(x => x.Activity)
			.Include(x => x.HotelPaymentMethods)
			.ThenInclude(x => x.PaymentMethod)
			.Include(x => x.HotelServices)
			.ThenInclude(x => x.Service)
			.Include(x => x.HotelStaffLanguages)
			.ThenInclude(x => x.StaffLanguage)
			.Include(x => x.Type)
			.Include(x => x.Country)
			.Include(x => x.Rooms)
			.ThenInclude(x => x.RoomImages)
			.Include(x => x.Rooms)
			.ThenInclude(x => x.Reservation)
			.AsSplitQuery()
			.FirstOrDefaultAsync(x=>x.Id==request.Id);
		if (hotel is null) throw new Exception("Hotel not found");
		HotelGetByIdQueryResponse dto = _mapper.Map<HotelGetByIdQueryResponse>(hotel);
		if (request.UserId is not null)
		{
			AppUser user = await _userManager.Users.Include(x => x.UserWishlistHotel).ThenInclude(x => x.Hotel).FirstOrDefaultAsync(x => x.Id == request.UserId);
			if (user is null) throw new Exception("User not found");

				if (user.UserWishlistHotel.Any(x => x.HotelId == dto.Id))
				{
					var matchingWishlistHotel = user.UserWishlistHotel.FirstOrDefault(x => x.HotelId == dto.Id);

					if (matchingWishlistHotel != null)
					{
						dto.WishlistItemId = matchingWishlistHotel.Id;
						dto.IsInWishlist = true;
					}
				}
			
		}
		return dto;
	}
	
}
