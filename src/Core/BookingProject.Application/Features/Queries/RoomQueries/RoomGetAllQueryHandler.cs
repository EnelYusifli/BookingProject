using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.RoomQueries;

public class RoomGetAllQueryHandler : IRequestHandler<RoomGetAllQueryRequest, ICollection<RoomGetAllQueryResponse>>
{
    private readonly IRoomRepository _repository;
    private readonly IMapper _mapper;
    private readonly IHotelRepository _hotelRepository;

    public RoomGetAllQueryHandler(IRoomRepository repository, IMapper mapper,IHotelRepository hotelRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _hotelRepository = hotelRepository;
    }

	public async Task<ICollection<RoomGetAllQueryResponse>> Handle(RoomGetAllQueryRequest request, CancellationToken cancellationToken)
	{
		Hotel hotel = await _hotelRepository.Table.FirstOrDefaultAsync(x => x.Id == request.HotelId);
		if (hotel is null)
			throw new NotFoundException("Hotel not found");

		ICollection<Room> act = await _repository.Table
		   .Where(x => x.HotelId == request.HotelId)
		   .Include(x => x.RoomImages)
		   .Include(x => x.Discounts)
		   .Include(x => x.Reservation)
		   .Include(x => x.Hotel)
		   .ThenInclude(x => x.Rooms)
		   .AsSplitQuery()
		   .ToListAsync();

		foreach (var room in act)
		{
			UpdateRoomDiscountedPrice(room);
		}

		if (act is null) throw new Exception("Room not found");

		ICollection<RoomGetAllQueryResponse> dtos = _mapper.Map<ICollection<RoomGetAllQueryResponse>>(act);

		return dtos;
	}

	private void UpdateRoomDiscountedPrice(Room room)
	{
		decimal discountedPrice = room.PricePerNight;
        int discPercent = 0;

		foreach (var discount in room.Discounts)
		{
			if ((!discount.IsDeactive) && discount.StartTime <= DateTime.Now && discount.EndTime >= DateTime.Now)
			{
				discountedPrice -= room.PricePerNight * (discount.Percent / 100m);
				discPercent=discount.Percent;
			}
		}
		room.DiscountedPricePerNight = discountedPrice;
        room.DiscountPercent= discPercent;
	}
}
