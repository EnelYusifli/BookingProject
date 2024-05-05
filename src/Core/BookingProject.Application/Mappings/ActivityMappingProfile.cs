using AutoMapper;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class ActivityMappingProfile:Profile
{
    public ActivityMappingProfile()
    {
        CreateMap<ActivityCreateCommandRequest,Activity>().ReverseMap();
    }
}
