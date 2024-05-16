using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewUpdateCommands;

public class ReviewUpdateCommandRequest:IRequest<ReviewUpdateCommandResponse>
{
    public required int Id { get; set; }
	[Range(0, 5, ErrorMessage = "StarPoint must be between 0 and 5.")]
	public int StarPoint { get; set; }
	[MaxLength(200)]
	public required string ReviewMessage { get; set; }
	public List<IFormFile>? ReviewImages { get; set; }
	public List<int>? DeletedImageFileIds { get; set; }
}
