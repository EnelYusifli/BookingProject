using AutoMapper;
using BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelUpdateCommands;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Application.Features.Queries.WishlistQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class HotelMappingProfile:Profile
{
    public HotelMappingProfile()
    {
		CreateMap<HotelCreateCommandRequest, Hotel>().ReverseMap();
			//.ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.AppUserId)).ReverseMap();
		CreateMap<HotelUpdateCommandRequest, Hotel>();
		CreateMap<Hotel, HotelGetAllQueryResponse>()
           .ForMember(dest => dest.ActivityNames, opt => opt.MapFrom(src => src.HotelActivities.Select(a => a.Activity.ActivityName)))
           .ForMember(dest => dest.ImageFileUrls, opt => opt.MapFrom(src => src.HotelImages.Select(a => a.Url)))
           .ForMember(dest => dest.AdvantageNames, opt => opt.MapFrom(src => src.HotelAdvantages.Select(a => a.AdvantageName)))
           .ForMember(dest => dest.PaymentMethodNames, opt => opt.MapFrom(src => src.HotelPaymentMethods.Select(p => p.PaymentMethod.PaymentMethodName)))
           .ForMember(dest => dest.ServiceNames, opt => opt.MapFrom(src => src.HotelServices.Select(s => s.Service.ServiceName)))
           .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
           .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.CountryName))
           .ForMember(dest => dest.StaffLanguageNames, opt => opt.MapFrom(src => src.HotelStaffLanguages.Select(l => l.StaffLanguage.StaffLanguageName)))
		   .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms)).ReverseMap() ;
		CreateMap<Hotel, HotelGetByIdQueryResponse>()
		 .ForMember(dest => dest.ActivityNames, opt => opt.MapFrom(src => src.HotelActivities.Select(a => a.Activity.ActivityName)))
		 .ForMember(dest => dest.ImageFileUrls, opt => opt.MapFrom(src => src.HotelImages.Select(a => a.Url)))
		 .ForMember(dest => dest.AdvantageNames, opt => opt.MapFrom(src => src.HotelAdvantages.Select(a => a.AdvantageName)))
		 .ForMember(dest => dest.PaymentMethodNames, opt => opt.MapFrom(src => src.HotelPaymentMethods.Select(p => p.PaymentMethod.PaymentMethodName)))
		 .ForMember(dest => dest.ServiceNames, opt => opt.MapFrom(src => src.HotelServices.Select(s => s.Service.ServiceName)))
		 .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
		 .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.CountryName))
		 .ForMember(dest => dest.StaffLanguageNames, opt => opt.MapFrom(src => src.HotelStaffLanguages.Select(l => l.StaffLanguage.StaffLanguageName)))
		 .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms)).ReverseMap();
		CreateMap<WishlistGetAllQueryResponse, UserWishlistHotel>().ReverseMap();
	}
}
