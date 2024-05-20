using MediatR;

namespace BookingProject.Application.Features.Commands.CountryCommands.CountrySoftDeleteCommands;

public class CountrySoftDeleteCommandRequest:IRequest<CountrySoftDeleteCommandResponse>
{
    public required int Id { get; set; }
}
