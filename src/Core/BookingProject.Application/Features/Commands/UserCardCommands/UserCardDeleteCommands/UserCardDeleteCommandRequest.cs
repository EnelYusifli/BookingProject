using MediatR;

namespace BookingProject.Application.Features.Commands.UserCardCommands.UserCardDeleteCommands;

public class UserCardDeleteCommandRequest:IRequest<UserCardDeleteCommandResponse>
{
    public int Id { get; set; }
}
