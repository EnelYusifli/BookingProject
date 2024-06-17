using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageCreateCommands;

public class AdvantageCreateCommandHandler : IRequestHandler<AdvantageCreateCommandRequest, AdvantageCreateCommandResponse>
{
    private readonly IAdvantageRepository _repository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public AdvantageCreateCommandHandler(IAdvantageRepository repository,IMapper mapper,IHotelRepository hotelRepository)
    {
        _repository = repository;
        _mapper = mapper;
		_hotelRepository = hotelRepository;
	}
    public async Task<AdvantageCreateCommandResponse> Handle(AdvantageCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null)
        {
            throw new NotFoundException("Request not found");
        }
        if (request.AdvantageName.IsNullOrEmpty())
        {
            throw new BadRequestException("Name cannot be null");
        }
        Hotel hotel = await _hotelRepository.Table.FirstOrDefaultAsync(x => x.Id == request.HotelId);
        if (hotel is null)
            throw new NotFoundException("Hotel not found");
        if (await _repository.Table.AnyAsync(x => x.AdvantageName.ToLower() == request.AdvantageName.ToLower() && x.HotelId==request.HotelId))
            throw new BadRequestException("Advantage Name is already exist");
        HotelAdvantage advantage=_mapper.Map<HotelAdvantage>(request);
        await _repository.CreateAsync(advantage);
        await _repository.CommitAsync();
        return new AdvantageCreateCommandResponse();
    }
}
