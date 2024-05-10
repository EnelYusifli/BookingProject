using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewUpdateCommands;

public class ReviewUpdateCommandRequest:IRequest<ReviewUpdateCommandResponse>
{
    public int Id { get; set; }
    public int StarPoint { get; set; }
	public string ReviewMessage { get; set; }
	public List<IFormFile>? ReviewImages { get; set; }
	public List<int>? DeletedImageFileIds { get; set; }
}
