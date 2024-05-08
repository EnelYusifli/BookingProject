using AutoMapper;
using BookingProject.Application.Features.Commands.RoomCommands.RoomCreateCommands;
using BookingProject.Application.Features.DTOs;
using BookingProject.Application.Features.Queries.RoomQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class RoomMappingProfile:Profile
{
    public RoomMappingProfile()
    {
        CreateMap<RoomCreateCommandRequest,Room>().ReverseMap();
        CreateMap<RoomCreateDto,Room>().ReverseMap();
        CreateMap<RoomGetAllQueryResponse, Room>().ReverseMap()
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.RoomImages.Select(a => a.Url))).ReverseMap();
    }
}
