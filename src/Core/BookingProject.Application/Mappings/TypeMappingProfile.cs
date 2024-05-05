using AutoMapper;
using BookingProject.Application.Features.Commands.TypeCommands.TypeCreateCommands;
using BookingProject.Application.Features.Commands.TypeCommands.TypeUpdateCommands;
using BookingProject.Application.Features.Queries.TypeQueries;

namespace BookingProject.Application.Mappings;

public class TypeMappingProfile : Profile
{
    public TypeMappingProfile()
    {
        CreateMap<TypeCreateCommandRequest, Domain.Entities.Type>().ReverseMap();
        CreateMap<TypeUpdateCommandRequest, Domain.Entities.Type>().ReverseMap();
        CreateMap<TypeGetAllQueryResponse, Domain.Entities.Type>().ReverseMap();
    }
}
