using AutoMapper;
using BookingProject.Application.Features.Commands.DiscountCommands.DiscountCreateCommands;
using BookingProject.Application.Features.Commands.OfferCommands.OfferCreateCommands;
using BookingProject.Application.Features.Commands.RoomCommands.RoomCreateCommands;
using BookingProject.Application.Features.Commands.RoomCommands.RoomUpdateCommands;
using BookingProject.Application.Features.DTOs;
using BookingProject.Application.Features.Queries.RoomQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class RoomMappingProfile:Profile
{
    public RoomMappingProfile()
    {
        CreateMap<RoomCreateCommandRequest,Room>().ReverseMap();
        CreateMap<RoomUpdateCommandRequest,Room>().ReverseMap();
        CreateMap<RoomCreateDto,Room>().ReverseMap();
        CreateMap<RoomGetAllQueryResponse, Room>().ReverseMap()
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.RoomImages.Select(a => a.Url)))
			.ForMember(dest => dest.Reservations, opt => opt.MapFrom(src => src.Reservation)).ReverseMap();
		CreateMap<Room, RoomGetByIdResponse>()
		   .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.RoomName))
		   .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
		   .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.Hotel.Id))
		   .ForMember(dest => dest.AdultCount, opt => opt.MapFrom(src => src.AdultCount))
		   .ForMember(dest => dest.Reservations, opt => opt.MapFrom(src => src.Reservation))
		   .ForMember(dest => dest.ChildCount, opt => opt.MapFrom(src => src.ChildCount))
		   .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.RoomImages.Select(hi => new ImageDto { Id = hi.Id, Url = hi.Url })))
		   .ForMember(dest => dest.ServiceFee, opt => opt.MapFrom(src => src.ServiceFee))
		   .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.PricePerNight))
		   .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
		   .ForMember(dest => dest.IsCancellable, opt => opt.MapFrom(src => src.IsCancellable))
		   .ForMember(dest => dest.CancelAfterDay, opt => opt.MapFrom(src => src.CancelAfterDay))
		   .ForMember(dest => dest.RoomImageUrls, opt => opt.MapFrom(src => src.RoomImages.Select(ri => ri.Url).ToList()))
		   .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
		   .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate));
		CreateMap<Discount, DiscountCreateCommandRequest>().ReverseMap();
		CreateMap<Offer, OfferCreateCommandRequest>().ReverseMap();
	}
}

