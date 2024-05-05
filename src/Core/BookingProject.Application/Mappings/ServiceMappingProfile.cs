using AutoMapper;
using BookingProject.Application.Features.Commands.ServiceCommands.ServiceCreateCommands;
using BookingProject.Application.Features.Commands.ServiceCommands.ServiceUpdateCommands;
using BookingProject.Application.Features.Queries.ServiceQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        CreateMap<ServiceCreateCommandRequest, Service>().ReverseMap();
        CreateMap<ServiceUpdateCommandRequest, Service>().ReverseMap();
        CreateMap<ServiceGetAllQueryResponse, Service>().ReverseMap();
    }
}
