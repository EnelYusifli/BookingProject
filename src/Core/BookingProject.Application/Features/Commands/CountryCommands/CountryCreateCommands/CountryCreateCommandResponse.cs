using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.CountryCommands.CountryCreateCommands;

public class CountryCreateCommandResponse
{
    [DataType(DataType.Text)]
    public string Text { get; set; } = "Country Created Successfully";
}
