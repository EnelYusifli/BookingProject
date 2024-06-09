using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.TermsOfServiceCommands;

public class TermsOfServiceUpdateCommandRequest:IRequest<TermsOfServiceUpdateCommandResponse>
{
	[MaxLength(200)]
	public string Title { get; set; }
	[MaxLength(5000)]
	public string Text { get; set; }
}
