using AutoMapper;
using BookingProject.Application.Features.Commands.UserCardCommands;
using BookingProject.Application.Features.Commands.UserCommands.UserUpdateCommands;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class UserMappingProfile:Profile
{
	public UserMappingProfile()
	{
		CreateMap<AppUser, UserUpdateCommandRequest>().ReverseMap();
		CreateMap<CardCreateCommandRequest, UserCard>().ReverseMap();
	}
}
