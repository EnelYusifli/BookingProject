using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

public class CountryGetByIdQueryHandler : IRequestHandler<CountryGetByIdQueryRequest, CountryGetByIdQueryResponse>
{
    private readonly ICountryRepository _repository;
    private readonly IMapper _mapper;

    public CountryGetByIdQueryHandler(ICountryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CountryGetByIdQueryResponse> Handle(CountryGetByIdQueryRequest request, CancellationToken cancellationToken)
    {
        Country Country = await _repository.GetByIdAsync(request.Id);
        if (Country is null) throw new Exception("Country not found");
        CountryGetByIdQueryResponse dto = _mapper.Map<CountryGetByIdQueryResponse>(Country);
        return dto;
    }
}
