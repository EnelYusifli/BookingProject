using BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetAllByUserQueries;
using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Features.Queries.RoomQueries;

public class RoomGetAllQueryResponse
{
    public string RoomName { get; set; }
    public int Id { get; set; }
    public required int HotelId { get; set; }
	public List<ReservationGetAllByUserQueryResponse>? Reservations { get; set; }
	public bool IsDeactive { get; set; }
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
	public int DiscountPercent { get; set; }
	public decimal ServiceFee { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal DiscountedPricePerNight { get; set; }
    public decimal Area { get; set; }
    public bool IsCancellable { get; set; }
    public int? CancelAfterDay { get; set; }
    public List<string> ImageUrls{ get; set; }
}
