using AutoMapper;
using BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;
using BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class AuthMappingProfile:Profile
{
    public AuthMappingProfile()
    {
        CreateMap<AuthLoginCommandRequest, AppUser>().ReverseMap();
        CreateMap<AuthRegisterCommandRequest, AppUser>().ReverseMap();
    }
}
