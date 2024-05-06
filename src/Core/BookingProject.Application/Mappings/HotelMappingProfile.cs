using AutoMapper;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class HotelMappingProfile:Profile
{
    public HotelMappingProfile()
    {
        CreateMap<HotelCreateCommandRequest, Hotel>().ReverseMap();
    }
}
