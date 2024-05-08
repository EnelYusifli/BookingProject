using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.RoomCommands.RoomSoftDeleteCommands;

public class RoomSoftDeleteCommandHandler : IRequestHandler<RoomSoftDeleteCommandRequest, RoomSoftDeleteCommandResponse>
{
    private readonly IRoomRepository _repository;

    public RoomSoftDeleteCommandHandler(IRoomRepository repository)
    {
        _repository = repository;
    }
    public async Task<RoomSoftDeleteCommandResponse> Handle(RoomSoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text = String.Empty;
        Room room = await _repository.Table
            .Where(x=>x.Hotel.IsDeactive==false)
            .Include(x => x.RoomImages)
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
        }
        return new RoomSoftDeleteCommandResponse();
    }
}
