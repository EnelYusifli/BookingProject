using BookingProject.Application.Features.Queries.RoomQueries;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class ReservationGetByIdViewModel
{
	public RoomGetByIdResponse Room { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public decimal TotalPrice { get; set; }
	public int DiscountPercent { get; set; }
	public bool IsPaid { get; set; }
	public bool IsCancelled { get; set; }
}
