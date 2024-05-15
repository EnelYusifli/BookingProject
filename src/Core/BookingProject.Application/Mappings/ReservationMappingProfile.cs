using AutoMapper;
using BookingProject.Application.Features.Commands.ReservationCommands.ReservationCreateCommands;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class ReservationMappingProfile:Profile
{
    public ReservationMappingProfile()
    {
        CreateMap<Reservation, ReservationCreateCommandRequest>().ReverseMap();
    }
}
