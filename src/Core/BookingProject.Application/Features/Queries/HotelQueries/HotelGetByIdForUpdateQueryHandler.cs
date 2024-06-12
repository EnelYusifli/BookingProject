using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetByIdForUpdateQueryHandler : IRequestHandler<HotelGetByIdForUpdateQueryRequest, HotelGetByIdForUpdateQueryResponse>
{
	private readonly IHotelRepository _repository;
	private readonly IMapper _mapper;

	public HotelGetByIdForUpdateQueryHandler(IHotelRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}
	public async Task<HotelGetByIdForUpdateQueryResponse> Handle(HotelGetByIdForUpdateQueryRequest request, CancellationToken cancellationToken)
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
			.Include(x => x.Country)
			.FirstOrDefaultAsync(x => x.Id == request.Id);
		if (hotel is null) throw new Exception("Hotel not found");
		HotelGetByIdForUpdateQueryResponse dto = _mapper.Map<HotelGetByIdForUpdateQueryResponse>(hotel);
		return dto;
	}

}
