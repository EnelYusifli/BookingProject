using AutoMapper;
using BookingProject.Application.Features.Commands.AboutCommands;
using BookingProject.Application.Features.Commands.MessageCommands.MessageCreateCommands;
using BookingProject.Application.Features.Commands.TermsOfServiceCommands;
using BookingProject.Application.Features.Queries.AboutQueries;
using BookingProject.Application.Features.Queries.MessageQueries;
using BookingProject.Application.Features.Queries.TermsOfServiceQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class AboutMappingProfile:Profile
{
    public AboutMappingProfile()
    {
        CreateMap<AboutUpdateCommandRequest,About>().ReverseMap();
        CreateMap<TermsOfServiceUpdateCommandRequest, TermsOfService>().ReverseMap();
        CreateMap<MessageCreateCommandRequest,Message>().ReverseMap();
		CreateMap<AboutGetByIdQueryResponse, About>().ReverseMap();
		CreateMap<TermsOfServiceGetByIdQueryResponse, TermsOfService>().ReverseMap();
		CreateMap<MessageGetAllQueryResponse, Message>().ReverseMap();
	}
}
