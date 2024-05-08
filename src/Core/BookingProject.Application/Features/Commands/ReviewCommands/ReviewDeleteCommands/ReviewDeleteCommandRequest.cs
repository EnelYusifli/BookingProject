using MediatR;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewDeleteCommands;

public class ReviewDeleteCommandRequest:IRequest<ReviewDeleteCommandResponse>
{
    public int Id { get; set; }
}
