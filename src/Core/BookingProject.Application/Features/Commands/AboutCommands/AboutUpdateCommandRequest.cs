using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.AboutCommands;

public class AboutUpdateCommandRequest:IRequest<AboutUpdateCommandResponse>
{
	[MaxLength(200)]
	public string StoryTitle { get; set; }
	[MaxLength(5000)]
	public string Story { get; set; }
}
