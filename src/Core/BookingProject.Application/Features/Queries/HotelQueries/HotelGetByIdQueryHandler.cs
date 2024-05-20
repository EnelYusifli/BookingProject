using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetByIdQueryHandler : IRequestHandler<HotelGetByIdQueryRequest, HotelGetByIdQueryResponse>
{
	private readonly IHotelRepository _repository;
	private readonly IMapper _mapper;

	public HotelGetByIdQueryHandler(IHotelRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}
	public async Task<HotelGetByIdQueryResponse> Handle(HotelGetByIdQueryRequest request, CancellationToken cancellationToken)
	{
		Hotel hotel = await _repository.Table
			.Include(x => x.HotelImages)
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
			.Include(x => x.Rooms)
			.ThenInclude(x => x.RoomImages)
			.Include(x => x.Rooms)
			.ThenInclude(x => x.Reservation)
			.FirstOrDefaultAsync(x=>x.Id==request.Id);
		if (hotel is null) throw new Exception("Hotel not found");
		HotelGetByIdQueryResponse dto = _mapper.Map<HotelGetByIdQueryResponse>(hotel);
		return dto;
	}
	
}
