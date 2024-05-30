using AutoMapper;
using BookingProject.Application.Features.Commands.ReviewCommands.ReviewCreateCommands;
using BookingProject.Application.Features.Commands.ReviewCommands.ReviewUpdateCommands;
using BookingProject.Application.Features.Queries.ReviewQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class CustomerReviewMappingProfile:Profile
{
    public CustomerReviewMappingProfile()
    {
        CreateMap<ReviewCreateCommandRequest, CustomerReview>().ReverseMap();
        CreateMap<ReviewUpdateCommandRequest, CustomerReview>().ReverseMap();
		CreateMap<ReviewGetAllQueryResponse, CustomerReview>().ReverseMap()
		   .ForMember(dest => dest.ReviewImageUrls, opt => opt.MapFrom(src => src.ReviewImages.Select(a => a.Url)))
		   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
		   .ForMember(dest => dest.UserPpUrl, opt => opt.MapFrom(src => src.User.ProfilePhotoUrl))
		   .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name)).ReverseMap();
	}
}
