using MediatR;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewReportCommands;

public class ReviewReportCommandRequest:IRequest<ReviewReportCommandResponse>
{
    public int Id { get; set; }
}
