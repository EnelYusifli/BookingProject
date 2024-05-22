using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewCreateCommands;

public class ReviewCreateCommandRequest:IRequest<ReviewCreateCommandResponse>
{
	public required int HotelId { get; set; }
	[Range(0, 5, ErrorMessage = "StarPoint must be between 0 and 5.")]
	public required int StarPoint { get; set; }
	[MaxLength(200)]
	public required string ReviewMessage { get; set; }
	public List<IFormFile>? ReviewImages { get; set; }
}
