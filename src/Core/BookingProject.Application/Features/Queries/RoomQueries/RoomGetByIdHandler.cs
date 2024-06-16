using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.RoomQueries;

public class RoomGetByIdHandler : IRequestHandler<RoomGetByIdRequest, RoomGetByIdResponse>
{
	private readonly IMapper _mapper;
	private readonly IRoomRepository _roomRepository;

	public RoomGetByIdHandler(IMapper mapper,IRoomRepository roomRepository)
    {
		_mapper = mapper;
		_roomRepository = roomRepository;
	}
    public async Task<RoomGetByIdResponse> Handle(RoomGetByIdRequest request, CancellationToken cancellationToken)
	{
		Room room=await _roomRepository.Table.Include(x => x.Reservation)
			.Include(x=>x.RoomImages).Include(x => x.Hotel)
		   .ThenInclude(x => x.Rooms).Include(x => x.Hotel)
		   .ThenInclude(x => x.HotelImages).Include(x=>x.Discounts)
		   .AsSplitQuery()
		   .FirstOrDefaultAsync(x=>x.Id==request.Id);
		if (room is null)
			throw new NotFoundException("Room not found");
		await UpdateRoomDiscountedPrice(room);
		RoomGetByIdResponse dto = _mapper.Map<RoomGetByIdResponse>(room);
		return dto;
	}
	private async Task UpdateRoomDiscountedPrice(Room room)
	{
		decimal discountedPrice = room.PricePerNight;
		int discPercent = 0;

		foreach (var discount in room.Discounts)
		{
			if (!discount.IsDeactive &&
				discount.StartTime <= DateTime.Now &&
				discount.EndTime >= DateTime.Now)
			{
				decimal discountPercentage = discount.Percent / 100m;
				decimal discountedAmount = room.PricePerNight * discountPercentage;
				int roundedDiscountedAmount = (int)Math.Round(discountedAmount);
				discountedPrice -= roundedDiscountedAmount;
				discPercent = discount.Percent;
			}
		}

		room.DiscountedPricePerNight = discountedPrice;
		room.DiscountPercent = discPercent;
		await _roomRepository.CommitAsync();
	}

}
