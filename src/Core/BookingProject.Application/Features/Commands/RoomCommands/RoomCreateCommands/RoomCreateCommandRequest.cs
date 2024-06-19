using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Features.Commands.RoomCommands.RoomCreateCommands;

public class RoomCreateCommandRequest:IRequest<RoomCreateCommandResponse>
{
    public string RoomName { get; set; }
    public required int HotelId { get; set; }
    public bool IsDeactive { get; set; }
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal PricePerNight { get; set; }
	public bool IsDepositNeeded { get; set; } = false;
	public decimal Area { get; set; }
    public bool IsCancellable { get; set; }
    public int? CancelAfterDay { get; set; }
    public List<IFormFile> ImageFiles { get; set; }
}
