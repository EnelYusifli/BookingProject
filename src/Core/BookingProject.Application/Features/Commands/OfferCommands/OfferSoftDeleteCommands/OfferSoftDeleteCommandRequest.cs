using MediatR;

namespace BookingProject.Application.Features.Commands.OfferCommands.OfferSoftDeleteCommands;

public class OfferSoftDeleteCommandRequest:IRequest<OfferSoftDeleteCommandResponse>
{
    public int Id { get; set; }
}
