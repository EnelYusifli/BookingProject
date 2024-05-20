using MediatR;

namespace BookingProject.Application.Features.Commands.CountryCommands.CountryDeleteCommands;

public class CountryDeleteCommandRequest:IRequest<CountryDeleteCommandResponse>
{
    public required int Id { get; set; }
}
