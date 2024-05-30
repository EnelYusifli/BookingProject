using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingProject.Application.Services.Implementations;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }
    public async Task IncreaseViewerCount(int id)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id);
        if (hotel is null)
            throw new NotFoundException("Hotel Not Found");

        hotel.ViewerCount += 1;
        await _hotelRepository.CommitAsync();
    }
}
