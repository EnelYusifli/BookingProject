namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class UserReservationsViewModel
{
	public List<ReservationGetViewModel> UpcomingReservations;
	public List<ReservationGetViewModel> CancelledReservations;
	public List<ReservationGetViewModel> CompletedReservations;
	public ReviewCreateViewModel Review;
}
