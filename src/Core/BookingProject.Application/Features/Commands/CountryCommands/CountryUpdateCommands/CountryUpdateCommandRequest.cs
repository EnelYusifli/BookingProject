using MediatR;

namespace BookingProject.Application.Features.Commands.CountryCommands.CountryUpdateCommands;

public class CountryUpdateCommandRequest:IRequest<CountryUpdateCommandResponse>
{
    public int Id { get; set; }
    public string CountryName { get; set; }
    public bool IsDeactive { get; set; }
}
