using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.HotelCommands.HotelSoftDeleteCommands;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelSoftDeleteCommands;

public class HotelSoftDeleteCommandHandler : IRequestHandler<HotelSoftDeleteCommandRequest, HotelSoftDeleteCommandResponse>
{
    private readonly IHotelRepository _repository;

    public HotelSoftDeleteCommandHandler(IHotelRepository repository)
    {
        _repository = repository;
    }
    public async Task<HotelSoftDeleteCommandResponse> Handle(HotelSoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text = String.Empty;
        Hotel hotel = await _repository.Table
            .Include(x=>x.HotelActivities)
            .Include(x => x.HotelAdvantages)
            .Include(x => x.HotelImages)
            .Include(x => x.HotelPaymentMethods)
            .Include(x => x.HotelServices)
            .Include(x => x.HotelStaffLanguages)
            .Include(x=>x.Rooms)
            .ThenInclude(x=>x.RoomImages)
            .Include(x=>x.CustomerReviews)
            .ThenInclude(x=>x.ReviewImages)
            .FirstOrDefaultAsync(x=>x.Id==request.Id);
        if (hotel is null) throw new NotFoundException("Hotel not found");
        if (hotel.IsDeactive == true)
        {
            hotel.IsDeactive = false;
            text = "Hotel Activated";
            foreach (var item in hotel.HotelStaffLanguages)
            {
                if (item.StaffLanguage.IsDeactive == false)
                    item.IsDeactive = false;
            }
            foreach (var item in hotel.Rooms)
            {
                    item.IsDeactive = false;
                foreach (var img in item.RoomImages)
                {
                    img.IsDeactive = false;
                }
            }
			foreach (var item in hotel.CustomerReviews)
			{
				item.IsDeactive = false;
                if(item.ReviewImages is not null)
                {
				foreach (var img in item.ReviewImages)
				{
					img.IsDeactive = false;
				}

                }
			}
			foreach (var item in hotel.HotelImages)
            {
                    item.IsDeactive = false;
            }
            foreach (var item in hotel.HotelActivities)
            {
                if (item.Activity.IsDeactive == false)
                    item.IsDeactive = false;
            }
            foreach (var item in hotel.HotelServices)
            {
                if (item.Service.IsDeactive == false)
                    item.IsDeactive = false;
            }
            foreach (var item in hotel.HotelAdvantages)
            {
                    item.IsDeactive = false;
            }
            foreach (var item in hotel.HotelPaymentMethods)
            {
                if (item.PaymentMethod.IsDeactive == false)
                    item.IsDeactive = false;
            }

        }
        else
        {
            hotel.IsDeactive = true;
            text = "Hotel Deactivated";
            foreach (var item in hotel.HotelStaffLanguages)
            {
                    item.IsDeactive = true;
            }
            foreach (var item in hotel.Rooms)
            {
                item.IsDeactive = true;
                foreach (var img in item.RoomImages)
                {
                    img.IsDeactive = true;
                }
            }
			foreach (var item in hotel.CustomerReviews)
			{
				item.IsDeactive = true;
                if (item.ReviewImages is not null)
                {
                    foreach (var img in item.ReviewImages)
                    {
                        img.IsDeactive = true;
                    }
                }
			}
			foreach (var item in hotel.HotelImages)
            {
                    item.IsDeactive = true;
            }
            foreach (var item in hotel.HotelActivities)
            {
                    item.IsDeactive = true;
            }
            foreach (var item in hotel.HotelServices)
            {
                    item.IsDeactive = true;
            }
            foreach (var item in hotel.HotelAdvantages)
            {
                    item.IsDeactive = true;
            }
            foreach (var item in hotel.HotelPaymentMethods)
            {
                    item.IsDeactive = true;
            }
        }
        await _repository.CommitAsync();
        return new HotelSoftDeleteCommandResponse()
        {
            Text = text
        };
    }
}
