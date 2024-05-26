using AutoMapper;
using BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageCreateCommands;
using BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageUpdateCommands;
using BookingProject.Application.Features.Queries.StaffLanguageQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class StaffLanguageMappingProfile : Profile
{
    public StaffLanguageMappingProfile()
    {
        CreateMap<StaffLanguageCreateCommandRequest, StaffLanguage>().ReverseMap();
        CreateMap<StaffLanguageUpdateCommandRequest, StaffLanguage>().ReverseMap();
        CreateMap<StaffLanguageGetAllQueryResponse, StaffLanguage>().ReverseMap();
        CreateMap<StaffLanguageGetByIdQueryResponse, StaffLanguage>().ReverseMap();
    }
}
