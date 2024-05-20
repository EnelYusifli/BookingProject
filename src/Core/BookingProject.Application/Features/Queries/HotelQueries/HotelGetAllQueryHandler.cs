using AutoMapper;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetAllQueryHandler : IRequestHandler<HotelGetAllQueryRequest, ICollection<HotelGetAllQueryResponse>>
{
    private readonly IHotelRepository _repository;
    private readonly IMapper _mapper;

    public HotelGetAllQueryHandler(IHotelRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<HotelGetAllQueryResponse>> Handle(HotelGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        ICollection<Hotel> act = await _repository.Table
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
            .Include(x => x.Rooms)
            .ThenInclude(x => x.RoomImages)
			.Include(x => x.Rooms)
			.ThenInclude(x => x.Reservation)
			.ToListAsync();
        if (act is null) throw new Exception("Hotel not found");
        ICollection<HotelGetAllQueryResponse> dtos = _mapper.Map<ICollection<HotelGetAllQueryResponse>>(act);
        return dtos;
    }
}
