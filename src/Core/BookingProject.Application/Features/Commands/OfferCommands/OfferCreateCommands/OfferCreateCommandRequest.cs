using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.OfferCommands.OfferCreateCommands;

	public class OfferCreateCommandRequest : IRequest<OfferCreateCommandResponse>
	{
		[Range(0, 100, ErrorMessage = "Percent value must be between 0 and 100.")]
		[Required]
		public int Percent { get; set; }

		[Required]
		public DateTime StartTime { get; set; }

		[Required]
		public DateTime EndTime { get; set; }
	}

	
