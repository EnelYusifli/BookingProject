using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.CountryCommands.CountryCreateCommands;

public class CountryCreateCommandHandler : IRequestHandler<CountryCreateCommandRequest, CountryCreateCommandResponse>
{
    private readonly ICountryRepository _repository;
    private readonly IMapper _mapper;

    public CountryCreateCommandHandler(ICountryRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<CountryCreateCommandResponse> Handle(CountryCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null)
        {
            throw new NotFoundException("Request not found");
        }
        if (request.CountryName.IsNullOrEmpty())
        {
            throw new BadRequestException("Name cannot be null");
        }
        if (await _repository.Table.AnyAsync(x => x.CountryName.ToLower() == request.CountryName.ToLower()))
            throw new BadRequestException("Country Name is already exist");
        Domain.Entities.Country Country=_mapper.Map<Domain.Entities.Country>(request);
        await _repository.CreateAsync(Country);
        await _repository.CommitAsync();
        return new CountryCreateCommandResponse();
    }
}
