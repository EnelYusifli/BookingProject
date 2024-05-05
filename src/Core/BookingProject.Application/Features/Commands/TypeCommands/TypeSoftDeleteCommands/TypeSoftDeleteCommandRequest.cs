using MediatR;

namespace BookingProject.Application.Features.Commands.TypeCommands.TypeSoftDeleteCommands;

public class TypeSoftDeleteCommandRequest:IRequest<TypeSoftDeleteCommandResponse>
{
    public required int Id { get; set; }
}
