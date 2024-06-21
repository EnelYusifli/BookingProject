using AutoMapper;
using BookingProject.Application.Features.Commands.ReservationCommands.ReservationCreateCommands;
using BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetAllByUserQueries;
using BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetByIdQueries;
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
	.ForMember(dest => dest.IsCancellable, opt => opt.MapFrom(src => src.Room.IsCancellable))
	.ForMember(dest => dest.CancelAfterDay, opt => opt.MapFrom(src => src.Room.CancelAfterDay))
	.ReverseMap();
		CreateMap<Reservation, ReservationGetByIdQueryResponse>()
	.ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
	.ReverseMap();
		CreateMap<Reservation, ReservationGetAllByOwnerQueryResponse>()
	.ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.Room.HotelId))
	.ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Room.Hotel.Name))
	.ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.RoomName))
	.ReverseMap();

	}
}
