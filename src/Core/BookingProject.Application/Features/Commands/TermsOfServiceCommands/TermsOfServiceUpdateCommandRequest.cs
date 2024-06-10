using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.TermsOfServiceCommands;

public class TermsOfServiceUpdateCommandRequest:IRequest<TermsOfServiceUpdateCommandResponse>
{
	[MaxLength(200)]
	[Required]
	public string Title { get; set; }
	[MaxLength(5000)]
	[Required]
	public string Text { get; set; }
}
