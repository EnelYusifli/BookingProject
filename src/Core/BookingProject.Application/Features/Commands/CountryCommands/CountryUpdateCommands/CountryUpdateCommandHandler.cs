using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.CountryCommands.CountryUpdateCommands;

public class CountryUpdateCommandHandler : IRequestHandler<CountryUpdateCommandRequest, CountryUpdateCommandResponse>
{
    private readonly ICountryRepository _repository;
    private readonly IMapper _mapper;

    public CountryUpdateCommandHandler(ICountryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<CountryUpdateCommandResponse> Handle(CountryUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Country Country=await _repository.GetByIdAsync(request.Id);
        if (Country is null) throw new NotFoundException("Country not found");
        if (request.CountryName.IsNullOrEmpty()) throw new BadRequestException("Name cannot be null");
        Domain.Entities.Country existAct = await _repository.Table.FirstOrDefaultAsync(x => x.CountryName.ToLower() == request.CountryName.ToLower());
        if (existAct is not null && existAct.CountryName != Country.CountryName )
            throw new BadRequestException("Country Name is already exist");
        Country = _mapper.Map(request,Country);
        Country.ModifiedDate=DateTime.Now;
        await _repository.CommitAsync();
        return new CountryUpdateCommandResponse();
    }
}
