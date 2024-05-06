using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.HotelCommands.HotelDeleteCommands;
using BookingProject.Application.Helpers.Extensions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelDeleteCommands;

public class HotelDeleteCommandHandler : IRequestHandler<HotelDeleteCommandRequest, HotelDeleteCommandResponse>
{
    private readonly IHotelRepository _repository;
    private readonly IConfiguration _configuration;

    public HotelDeleteCommandHandler(IHotelRepository repository,IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }
    public async Task<HotelDeleteCommandResponse> Handle(HotelDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        Hotel hotel = await _repository.Table.Include(x=>x.HotelImages).FirstOrDefaultAsync(x=>x.Id==request.Id);
        if (hotel is null) throw new NotFoundException("Hotel not found");
        if (hotel.HotelImages is null) throw new NotFoundException("Hotel image not found");
        SaveFileExtension.Initialize(_configuration);
        foreach (var image in hotel.HotelImages)
        {
            await SaveFileExtension.DeleteFileAsync(image.Url);
        }
        _repository.Delete(hotel);
        await _repository.CommitAsync();
        return new HotelDeleteCommandResponse();
    }
}
