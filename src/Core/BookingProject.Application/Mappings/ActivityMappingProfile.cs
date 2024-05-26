using AutoMapper;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityUpdateCommands;
using BookingProject.Application.Features.Queries.ActivityQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class ActivityMappingProfile:Profile
{
    public ActivityMappingProfile()
    {
        CreateMap<ActivityCreateCommandRequest,Activity>().ReverseMap();
        CreateMap<ActivityUpdateCommandRequest,Activity>().ReverseMap();
        CreateMap<ActivityGetAllQueryResponse, Activity>().ReverseMap();
        CreateMap<ActivityGetByIdQueryResponse, Activity>().ReverseMap();
    }
}
