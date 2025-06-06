﻿namespace BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetAllByUserQueries;

public class ReservationGetAllByUserQueryResponse
{
	public int RoomId { get; set; }
	public int Id { get; set; }
	public string RoomName { get; set; }
	public int HotelId { get; set; }
	public string HotelName { get; set; }
	public string AppUserId { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime EndTime { get; set; }
	public bool IsPaid { get; set; } = false;
	public bool IsDeactive { get; set; } = false;
	public bool IsCancelled { get; set; } = false;
	public bool? IsCancellable { get; set; } = false;
	public int? CancelAfterDay { get; set; } = 0;
}
