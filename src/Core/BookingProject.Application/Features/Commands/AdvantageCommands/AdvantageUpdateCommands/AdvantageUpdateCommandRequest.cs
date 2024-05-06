using MediatR;

namespace BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageUpdateCommands;

public class AdvantageUpdateCommandRequest:IRequest<AdvantageUpdateCommandResponse>
{
    public int Id { get; set; }
    public string AdvantageName { get; set; }
    public int HotelId { get; set; }
    public bool IsDeactive { get; set; }
}
