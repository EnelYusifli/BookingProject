using AutoMapper;
using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageCreateCommands;
using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageUpdateCommands;
using BookingProject.Application.Features.Queries.AdvantageQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class AdvantageMappingProfile : Profile
{
    public AdvantageMappingProfile()
    {
        CreateMap<AdvantageCreateCommandRequest, HotelAdvantage>().ReverseMap();
        CreateMap<AdvantageUpdateCommandRequest, HotelAdvantage>().ReverseMap();
        CreateMap<AdvantageGetAllQueryResponse, HotelAdvantage>().ReverseMap();
    }
}
