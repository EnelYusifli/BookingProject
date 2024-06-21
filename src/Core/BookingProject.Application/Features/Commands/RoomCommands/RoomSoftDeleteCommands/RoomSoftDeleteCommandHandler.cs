using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.ReservationCommands.ReservationCancelCommands;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.RoomCommands.RoomSoftDeleteCommands;

public class RoomSoftDeleteCommandHandler : IRequestHandler<RoomSoftDeleteCommandRequest, RoomSoftDeleteCommandResponse>
{
    private readonly IRoomRepository _repository;
    private readonly IMediator _mediator;

    public RoomSoftDeleteCommandHandler(IRoomRepository repository,IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }
    public async Task<RoomSoftDeleteCommandResponse> Handle(RoomSoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text = String.Empty;
        Room room = await _repository.Table
            .Where(x=>x.Hotel.IsDeactive==false)
            .Include(x => x.RoomImages)
            .Include(x=>x.Reservation)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == request.Id);
        if (room is null) throw new NotFoundException("Room not found");
        if (room.IsDeactive == true)
        {
            room.IsDeactive = false;
            text = "Room Activated";
            foreach (var item in room.RoomImages)
            {
                item.IsDeactive = false;
            }
        }
        if (room.IsDeactive == false)
        {
            room.IsDeactive = true;
            text = "Room Deactivated";
            foreach (var item in room.RoomImages)
            {
                item.IsDeactive = true;
            }
            foreach (var reservation in room.Reservation.Where(x => x.StartTime > DateTime.Now))
            {
                ReservationCancelCommandRequest reservRequest = new()
                {
                    ReservationId = reservation.Id
                };
                await _mediator.Send(reservRequest);
            }
        }
        return new RoomSoftDeleteCommandResponse();
    }
}
