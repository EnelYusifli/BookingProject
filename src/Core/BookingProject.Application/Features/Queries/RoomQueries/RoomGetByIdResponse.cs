using BookingProject.Application.Features.DTOs;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Features.Queries.RoomQueries;

public class RoomGetByIdResponse
{
	public DateTime CreatedDate { get; set; } 
	public DateTime ModifiedDate { get; set; }
	public string RoomName { get; set; }
	public int Id { get; set; }
	public int DiscountPercent { get; set; }
	public string HotelName { get; set; }
	public int HotelId { get; set; }
	public int AdultCount { get; set; }
	public int ChildCount { get; set; }
	public decimal ServiceFee { get; set; }
	public decimal PricePerNight { get; set; }
	public decimal DiscountedPricePerNight { get; set; }
	public decimal Area { get; set; }
	public bool IsCancellable { get; set; }
	public int? CancelAfterDay { get; set; }
	public List<string> RoomImageUrls { get; set; }
	public List<ImageDto> Images { get; set; }	

}
