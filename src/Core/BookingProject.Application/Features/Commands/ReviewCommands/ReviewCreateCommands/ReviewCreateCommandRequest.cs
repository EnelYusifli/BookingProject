using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewCreateCommands;

public class ReviewCreateCommandRequest:IRequest<ReviewCreateCommandResponse>
{
	public int HotelId { get; set; }
	public string UserId { get; set; }
	public int StarPoint { get; set; }
	public string ReviewMessage { get; set; }
	public List<IFormFile>? ReviewImages { get; set; }
}
