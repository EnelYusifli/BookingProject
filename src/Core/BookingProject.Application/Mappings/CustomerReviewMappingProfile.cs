using AutoMapper;
using BookingProject.Application.Features.Commands.ReviewCommands.ReviewCreateCommands;
using BookingProject.Application.Features.Queries.ReviewQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class CustomerReviewMappingProfile:Profile
{
    public CustomerReviewMappingProfile()
    {
        CreateMap<ReviewCreateCommandRequest, CustomerReview>().ReverseMap();
		CreateMap<ReviewGetAllQueryResponse, CustomerReview>().ReverseMap()
		   .ForMember(dest => dest.ReviewImageUrls, opt => opt.MapFrom(src => src.ReviewImages.Select(a => a.Url))).ReverseMap();
	}
}
