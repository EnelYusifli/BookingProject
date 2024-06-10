using MediatR;

namespace BookingProject.Application.Features.Commands.FAQCommands.FAQDeleteCommands;

public class FAQDeleteCommandRequest:IRequest<FAQDeleteCommandResponse>
{
    public int Id { get; set; }
}
