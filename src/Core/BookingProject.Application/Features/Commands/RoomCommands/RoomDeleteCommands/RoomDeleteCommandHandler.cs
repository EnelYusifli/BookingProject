using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.RoomCommands.RoomDeleteCommands;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookingProject.Application.Features.Commands.RoomCommands.RoomDeleteCommands;

internal class RoomDeleteCommandHandler : IRequestHandler<RoomDeleteCommandRequest, RoomDeleteCommandResponse>
{
    private readonly IRoomRepository _repository;
    //private readonly IConfiguration _configuration;
    private readonly ICloudinaryService _cloudinaryService;

    public RoomDeleteCommandHandler(IRoomRepository repository,/* IConfiguration configuration*/ICloudinaryService cloudinaryService)
    {
        _repository = repository;
        //_configuration = configuration;
        _cloudinaryService = cloudinaryService;
    }
    public async Task<RoomDeleteCommandResponse> Handle(RoomDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        Room room = await _repository.Table.Include(x => x.RoomImages).FirstOrDefaultAsync(x => x.Id == request.Id);
        if (room is null) throw new NotFoundException("Room not found");
        if (room.RoomImages is null) throw new NotFoundException("Room image not found");
        //SaveFileExtension.Initialize(_configuration);
        foreach (var image in room.RoomImages)
        {
            //await SaveFileExtension.DeleteFileAsync(image.Url);
            await _cloudinaryService.FileDeleteAsync(image.Url);
        }
        _repository.Delete(room);
        await _repository.CommitAsync();
        return new RoomDeleteCommandResponse();
    }
}
