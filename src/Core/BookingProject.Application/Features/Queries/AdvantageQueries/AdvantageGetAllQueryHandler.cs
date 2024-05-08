using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.AdvantageQueries;

public class AdvantageGetAllQueryHandler : IRequestHandler<AdvantageGetAllQueryRequest, ICollection<AdvantageGetAllQueryResponse>>
{
    private readonly IAdvantageRepository _repository;
    private readonly IMapper _mapper;
    private readonly IHotelRepository _hotelRepository;

    public AdvantageGetAllQueryHandler(IAdvantageRepository repository, IMapper mapper,IHotelRepository hotelRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _hotelRepository = hotelRepository;
    }
    public async Task<ICollection<AdvantageGetAllQueryResponse>> Handle(AdvantageGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        Hotel hotel = await _hotelRepository.Table.Where(x => x.IsDeactive == false).FirstOrDefaultAsync(x => x.Id == request.HotelId);
        if (hotel is null)
            throw new NotFoundException("Hotel not found");
        ICollection<HotelAdvantage> adv = await _repository.Table.Where(x=>x.HotelId==request.HotelId).ToListAsync();
        if (adv is null) throw new Exception("Advantage not found");
        ICollection<AdvantageGetAllQueryResponse> dtos = _mapper.Map<ICollection<AdvantageGetAllQueryResponse>>(adv);
        return dtos;
    }
}
