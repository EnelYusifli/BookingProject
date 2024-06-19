namespace BookingProject.Application.Features.Commands.ReservationCommands.ReservationCreateCommands;

public class ReservationCreateCommandResponse
{
    public string Text { get; set; } = "Reservation created";
    public decimal TotalPrice { get; set; }
}
