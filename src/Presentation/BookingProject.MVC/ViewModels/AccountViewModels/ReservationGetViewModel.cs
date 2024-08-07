﻿namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class ReservationGetViewModel
{
	public int RoomId { get; set; }
	public int Id { get; set; }
	public int HotelId { get; set; }
	public string RoomName { get; set; }
	public string HotelName { get; set; }
	public string AppUserId { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public bool IsPaid { get; set; }
	public bool IsDeactive { get; set; }
	public bool IsCancelled { get; set; }
	public bool IsCancellable { get; set; } = false;
	public int CancelAfterDay { get; set; } = 0;
	public DateTime CreatedDate { get; set; }

}
