using MediatR;

namespace BookingProject.Application.Features.Commands.DiscountCommands.DiscountSoftDeleteCommands;

public class DiscountSoftDeleteCommandRequest:IRequest<DiscountSoftDeleteCommandResponse>
{
    public int Id { get; set; }
}
