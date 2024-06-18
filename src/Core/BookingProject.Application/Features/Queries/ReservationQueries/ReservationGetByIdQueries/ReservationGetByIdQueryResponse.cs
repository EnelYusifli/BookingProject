using BookingProject.Application.Features.Queries.RoomQueries;

namespace BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetByIdQueries;

public class ReservationGetByIdQueryResponse
{
	public RoomGetByIdResponse Room {  get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public decimal TotalPrice { get; set; }
	public int DiscountPercent { get; set; }
	public bool IsPaid { get; set; } 
	public bool IsCancelled { get; set; } 
}
