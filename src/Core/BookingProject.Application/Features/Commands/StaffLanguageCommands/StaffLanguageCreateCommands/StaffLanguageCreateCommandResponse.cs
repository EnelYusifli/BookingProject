using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageCreateCommands;

public class StaffLanguageCreateCommandResponse
{
    [DataType(DataType.Text)]
    public string Text { get; set; } = "StaffLanguage Created Successfully";
}
