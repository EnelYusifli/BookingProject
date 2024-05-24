using AutoMapper;
using BookingProject.Application.Features.Commands.UserCardCommands;
using BookingProject.Application.Features.Commands.UserCommands.UserUpdateCommands;
using BookingProject.Application.Features.Queries.UserQueries;
using BookingProject.Application.Features.Queries.WishlistQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class UserMappingProfile:Profile
{
	public UserMappingProfile()
	{
		CreateMap<WishlistGetAllQueryResponse, UserWishlistHotel>().ForMember(dest => dest.Hotel, opt => opt.MapFrom(src => src.Hotel)).ReverseMap();
		CreateMap<AppUser, UserUpdateCommandRequest>().ReverseMap();
		CreateMap<CardCreateCommandRequest, UserCard>().ReverseMap();
		CreateMap<GetAllUsersQueryResponse, AppUser>().ReverseMap();
		CreateMap<GetUserByIdQueryResponse, AppUser>().ReverseMap();
	}
}
