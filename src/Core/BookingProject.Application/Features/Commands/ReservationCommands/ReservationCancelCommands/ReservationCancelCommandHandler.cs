using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.ReservationCommands.ReservationCancelCommands;

public class ReservationCancelCommandHandler : IRequestHandler<ReservationCancelCommandRequest, ReservationCancelCommandResponse>
{
	private readonly IReservationRepository _repository;
    private readonly IEmailService _emailService;

    public ReservationCancelCommandHandler(IReservationRepository repository,IEmailService emailService)
    {
		_repository = repository;
        _emailService = emailService;
    }
    public async Task<ReservationCancelCommandResponse> Handle(ReservationCancelCommandRequest request, CancellationToken cancellationToken)
	{
		var reservation = await _repository.Table.Include(x=>x.Room).Include(x=>x.AppUser).FirstOrDefaultAsync(x=>x.Id==request.ReservationId);
		if (reservation is null || reservation.IsDeactive==true || reservation.IsCancelled==true)
			throw new NotFoundException("Reservation not found");
		reservation.IsCancelled = true;
		reservation.IsDeactive = true;
		if (!await _repository.Table.AnyAsync(x => x.RoomId == reservation.RoomId))
			reservation.Room.IsReserved = false;
		await _repository.CommitAsync();
        string subject = "Cancelled Reservation";
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email", "cancelledreservation.html");
        string html = File.ReadAllText(filePath);
        html = html.Replace("{{id}}", reservation.Id.ToString());
        html = html.Replace("{{checkin}}", reservation.StartTime.ToString("dd/MM/yyyy"));
        html = html.Replace("{{checkout}}", reservation.EndTime.ToString("dd/MM/yyyy"));
        html = html.Replace("{{room}}", reservation.Room.RoomName);
        await _emailService.SendEmail(reservation.AppUser.Email, subject, html);
        return new ReservationCancelCommandResponse();
	}
}
