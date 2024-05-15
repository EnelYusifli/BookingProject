using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.ReservationCommands.ReservationCancelCommands;

public class ReservationCancelCommandHandler : IRequestHandler<ReservationCancelCommandRequest, ReservationCancelCommandResponse>
{
	private readonly IReservationRepository _repository;

	public ReservationCancelCommandHandler(IReservationRepository repository)
    {
		_repository = repository;
	}
    public async Task<ReservationCancelCommandResponse> Handle(ReservationCancelCommandRequest request, CancellationToken cancellationToken)
	{
		var reservation = await _repository.Table.Include(x=>x.Room).FirstOrDefaultAsync(x=>x.Id==request.ReservationId);
		if (reservation is null || reservation.IsDeactive==true || reservation.IsCancelled==true)
			throw new NotFoundException("Reservation not found");
		reservation.IsCancelled = true;
		reservation.IsDeactive = true;
		if (!await _repository.Table.AnyAsync(x => x.RoomId == reservation.RoomId))
			reservation.Room.IsReserved = false;
		await _repository.CommitAsync();
		return new ReservationCancelCommandResponse();
	}
}
