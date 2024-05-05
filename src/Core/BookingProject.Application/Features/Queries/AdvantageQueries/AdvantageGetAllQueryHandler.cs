using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using System.Reflection.Metadata;

namespace BookingProject.Application.Features.Queries.AdvantageQueries;

public class AdvantageGetAllQueryHandler : IRequestHandler<AdvantageGetAllQueryRequest, ICollection<AdvantageGetAllQueryResponse>>
{
    private readonly IAdvantageRepository _repository;
    private readonly IMapper _mapper;

    public AdvantageGetAllQueryHandler(IAdvantageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<AdvantageGetAllQueryResponse>> Handle(AdvantageGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        ICollection<HotelAdvantage> adv = await _repository.GetAllAsync();
        if (adv is null) throw new Exception("Advantage not found");
        ICollection<AdvantageGetAllQueryResponse> dtos = _mapper.Map<ICollection<AdvantageGetAllQueryResponse>>(adv);
        return dtos;
    }
}
