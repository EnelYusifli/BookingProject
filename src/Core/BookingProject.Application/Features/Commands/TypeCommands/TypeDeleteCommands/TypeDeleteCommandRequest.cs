using MediatR;

namespace BookingProject.Application.Features.Commands.TypeCommands.TypeDeleteCommands;

public class TypeDeleteCommandRequest:IRequest<TypeDeleteCommandResponse>
{
    public required int Id { get; set; }
}
