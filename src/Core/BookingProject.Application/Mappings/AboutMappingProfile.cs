using AutoMapper;
using BookingProject.Application.Features.Commands.AboutCommands;
using BookingProject.Application.Features.Queries.AboutQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class AboutMappingProfile:Profile
{
    public AboutMappingProfile()
    {
        CreateMap<AboutUpdateCommandRequest,About>().ReverseMap();
		CreateMap<AboutGetByIdQueryResponse, About>().ReverseMap();
	}
}
