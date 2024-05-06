using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageCreateCommands;

public class AdvantageCreateCommandRequest:IRequest<AdvantageCreateCommandResponse>
{
    [DataType(DataType.Text)]
    public string AdvantageName { get; set; }
    public required int HotelId { get; set; }
    public bool IsDeactive { get; set; } = false;
}
