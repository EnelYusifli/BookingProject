using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.TypeCommands.TypeCreateCommands;

public class TypeCreateCommandRequest:IRequest<TypeCreateCommandResponse>
{
    [DataType(DataType.Text)]
    public string TypeName { get; set; }
    public bool IsDeactive { get; set; } = false;
}
