using AutoMapper;
using BookingProject.Application.Features.Commands.ReservationCommands.ReservationCreateCommands;
using BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetAllByUserQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class ReservationMappingProfile:Profile
{
    public ReservationMappingProfile()
    {
        CreateMap<Reservation, ReservationCreateCommandRequest>().ReverseMap();
		CreateMap<Reservation, ReservationGetAllByUserQueryResponse>()
	.ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.Room.HotelId))
	.ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Room.Hotel.Name))
	.ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.RoomName))
	.ReverseMap();

	}
}
