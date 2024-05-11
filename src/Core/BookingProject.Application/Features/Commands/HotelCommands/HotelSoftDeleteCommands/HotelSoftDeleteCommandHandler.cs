using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.HotelCommands.HotelSoftDeleteCommands;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelSoftDeleteCommands
{
	public class HotelSoftDeleteCommandHandler : IRequestHandler<HotelSoftDeleteCommandRequest, HotelSoftDeleteCommandResponse>
	{
		private readonly IHotelRepository _repository;

		public HotelSoftDeleteCommandHandler(IHotelRepository repository)
		{
			_repository = repository;
		}

		public async Task<HotelSoftDeleteCommandResponse> Handle(HotelSoftDeleteCommandRequest request, CancellationToken cancellationToken)
		{
			string text = string.Empty;
			Hotel hotel = await _repository.Table
				.Include(x => x.HotelActivities)
				.Include(x => x.HotelAdvantages)
				.Include(x => x.HotelImages)
				.Include(x => x.HotelPaymentMethods)
				.Include(x => x.HotelServices)
				.Include(x => x.HotelStaffLanguages)
				.Include(x => x.Rooms)
					.ThenInclude(x => x.RoomImages)
				.Include(x => x.CustomerReviews)
					.ThenInclude(x => x.ReviewImages)
				.FirstOrDefaultAsync(x => x.Id == request.Id);

			if (hotel is null)
				throw new NotFoundException("Hotel not found");

			if (hotel.IsDeactive)
			{
				hotel.IsDeactive = false;
				text = "Hotel Activated";

				foreach (var staffLanguage in hotel.HotelStaffLanguages)
				{
					if (staffLanguage.StaffLanguage?.IsDeactive == false)
						staffLanguage.IsDeactive = false;
				}

				foreach (var room in hotel.Rooms)
				{
					room.IsDeactive = false;
					foreach (var img in room.RoomImages)
					{
						img.IsDeactive = false;
					}
				}

				foreach (var review in hotel.CustomerReviews)
				{
					review.IsDeactive = false;
					foreach (var img in review.ReviewImages ?? Enumerable.Empty<ReviewImage>())
					{
						if (img != null)
						{
							img.IsDeactive = false;
						}
					}
				}

				foreach (var img in hotel.HotelImages)
				{
					img.IsDeactive = false;
				}

				foreach (var activity in hotel.HotelActivities)
				{
					if (activity.Activity?.IsDeactive == false)
						activity.IsDeactive = false;
				}

				foreach (var service in hotel.HotelServices)
				{
					if (service.Service?.IsDeactive == false)
						service.IsDeactive = false;
				}

				foreach (var advantage in hotel.HotelAdvantages)
				{
					advantage.IsDeactive = false;
				}

				foreach (var paymentMethod in hotel.HotelPaymentMethods)
				{
					if (paymentMethod.PaymentMethod?.IsDeactive == false)
						paymentMethod.IsDeactive = false;
				}
			}
			else
			{
				hotel.IsDeactive = true;
				text = "Hotel Deactivated";

				foreach (var staffLanguage in hotel.HotelStaffLanguages)
				{
					staffLanguage.IsDeactive = true;
				}

				foreach (var room in hotel.Rooms)
				{
					room.IsDeactive = true;
					foreach (var img in room.RoomImages)
					{
						img.IsDeactive = true;
					}
				}

				foreach (var review in hotel.CustomerReviews)
				{
					review.IsDeactive = true;
					foreach (var img in review.ReviewImages ?? Enumerable.Empty<ReviewImage>())
					{
						if (img != null)
						{
							img.IsDeactive = true;
						}
					}
				}

				foreach (var img in hotel.HotelImages)
				{
					img.IsDeactive = true;
				}

				foreach (var activity in hotel.HotelActivities)
				{
					activity.IsDeactive = true;
				}

				foreach (var service in hotel.HotelServices)
				{
					service.IsDeactive = true;
				}

				foreach (var advantage in hotel.HotelAdvantages)
				{
					advantage.IsDeactive = true;
				}

				foreach (var paymentMethod in hotel.HotelPaymentMethods)
				{
					paymentMethod.IsDeactive = true;
				}
			}

			await _repository.CommitAsync();

			return new HotelSoftDeleteCommandResponse()
			{
				Text = text
			};
		}
	}
}
