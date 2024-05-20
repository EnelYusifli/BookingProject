using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.CountryCommands.CountryCreateCommands;

public class CountryCreateCommandRequest:IRequest<CountryCreateCommandResponse>
{
    [DataType(DataType.Text)]
    public string CountryName { get; set; }
    public bool IsDeactive { get; set; } = false;
}
