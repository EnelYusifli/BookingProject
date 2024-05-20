using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.CountryCommands.CountryDeleteCommands;

public class CountryDeleteCommandHandler : IRequestHandler<CountryDeleteCommandRequest, CountryDeleteCommandResponse>
{
    private readonly ICountryRepository _repository;

    public CountryDeleteCommandHandler(ICountryRepository repository)
    {
        _repository = repository;
    }
    public async Task<CountryDeleteCommandResponse> Handle(CountryDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Country Country = await _repository.GetByIdAsync(request.Id);
        if (Country is null) throw new NotFoundException("Country not found");
        _repository.Delete(Country);
        await _repository.CommitAsync();
        return new CountryDeleteCommandResponse();
    }
}
