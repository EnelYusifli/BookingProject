using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.FAQCommands.FAQCreateCommands;

public class FAQCreateCommandRequest:IRequest<FAQCreateCommandResponse>
{
    [MaxLength(200)]
    [Required]
    public string Question { get; set; }
    [MaxLength(1000)]
    [Required]
    public string Answer { get; set; }
}
