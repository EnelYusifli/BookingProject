﻿using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Features.Queries.RoomQueries;

public class RoomGetAllQueryResponse
{
    public string RoomName { get; set; }
    public required int HotelId { get; set; }
    public bool IsDeactive { get; set; }
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal Area { get; set; }
    public bool IsCancellable { get; set; }
    public int? CancelAfterDay { get; set; }
    public List<string> ImageUrls{ get; set; }
}