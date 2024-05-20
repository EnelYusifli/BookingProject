using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using System.Reflection.Metadata;

namespace BookingProject.Application.Features.Queries.CountryQueries;

public class CountryGetAllQueryHandler : IRequestHandler<CountryGetAllQueryRequest, ICollection<CountryGetAllQueryResponse>>
{
    private readonly ICountryRepository _repository;
    private readonly IMapper _mapper;

    public CountryGetAllQueryHandler(ICountryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<CountryGetAllQueryResponse>> Handle(CountryGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        ICollection<Domain.Entities.Country> act = await _repository.GetAllAsync();
        if (act is null) throw new Exception("Country not found");
        ICollection<CountryGetAllQueryResponse> dtos = _mapper.Map<ICollection<CountryGetAllQueryResponse>>(act);
        return dtos;
    }
}
