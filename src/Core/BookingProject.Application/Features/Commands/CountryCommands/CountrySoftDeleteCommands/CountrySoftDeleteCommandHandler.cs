using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.CountryCommands.CountrySoftDeleteCommands;

public class CountrySoftDeleteCommandHandler : IRequestHandler<CountrySoftDeleteCommandRequest,CountrySoftDeleteCommandResponse>
{
    private readonly ICountryRepository _repository;
    private readonly IMapper _mapper;

    public CountrySoftDeleteCommandHandler(ICountryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<CountrySoftDeleteCommandResponse> Handle(CountrySoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text=String.Empty;
        Country country= await _repository.Table.Include(x=>x.Hotels).FirstOrDefaultAsync(x=>x.Id==request.Id);
        if (country is null) throw new NotFoundException("Country not found");
        if (country.IsDeactive == true)
        {
			country.IsDeactive = false;
            text = "Country Activated";
        }
        else
        {
			country.IsDeactive = true;
            text = "Country Deactivated";
            foreach (var item in country.Hotels)
            {
                item.IsDeactive = true;
            }
        }
        await _repository.CommitAsync();
        return new CountrySoftDeleteCommandResponse()
        {
            Text = text
        };

    }
}
