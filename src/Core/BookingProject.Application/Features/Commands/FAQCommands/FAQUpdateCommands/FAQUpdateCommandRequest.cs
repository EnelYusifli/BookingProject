using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.FAQCommands.FAQUpdateCommands;

public class FAQUpdateCommandRequest:IRequest<FAQUpdateCommandResponse>
{
    [Required]
    [MaxLength(200)]
    public string Question { get; set; }
    public int Id { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Answer { get; set; }
}
