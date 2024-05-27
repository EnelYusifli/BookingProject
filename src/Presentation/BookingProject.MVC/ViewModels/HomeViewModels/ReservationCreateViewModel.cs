namespace BookingProject.MVC.ViewModels.HomeViewModels;

public class ReservationCreateViewModel
{
	public required int RoomId { get; set; }
	public bool IsPaid { get; set; }
	public required DateTime StartTime { get; set; }
	public required DateTime EndTime { get; set; }
}
