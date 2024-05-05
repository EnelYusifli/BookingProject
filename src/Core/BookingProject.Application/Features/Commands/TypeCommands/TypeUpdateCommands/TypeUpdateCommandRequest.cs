using MediatR;

namespace BookingProject.Application.Features.Commands.TypeCommands.TypeUpdateCommands;

public class TypeUpdateCommandRequest:IRequest<TypeUpdateCommandResponse>
{
    public int Id { get; set; }
    public string TypeName { get; set; }
    public bool IsDeactive { get; set; }
}
