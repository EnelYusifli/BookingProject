using BookingProject.Application.Features.Commands.RoomCommands.RoomCreateCommands;
using BookingProject.Application.Features.DTOs;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands;

public class HotelCreateCommandRequest:IRequest<HotelCreateCommandResponse>
{
    public int TypeId { get; set; }
    public int CountryId { get; set; }
    public string AppUserId { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public List<RoomCreateDto> RoomCreateDtos { get; set; }
    public List<string>? HotelAdvantageNames { get; set; }
    public List<IFormFile> ImageFiles { get; set; }
    public List<int>? StaffLanguageIds { get; set; }
    public List<int>? ServiceIds { get; set; }
    public List<int>? PaymentMethodIds { get; set; }
    public List<int>? ActivityIds { get; set; }
}
