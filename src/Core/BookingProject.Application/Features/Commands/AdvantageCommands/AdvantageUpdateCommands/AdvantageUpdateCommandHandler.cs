using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageUpdateCommands;

public class AdvantageUpdateCommandHandler : IRequestHandler<AdvantageUpdateCommandRequest, AdvantageUpdateCommandResponse>
{
    private readonly IAdvantageRepository _repository;
    private readonly IMapper _mapper;

    public AdvantageUpdateCommandHandler(IAdvantageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<AdvantageUpdateCommandResponse> Handle(AdvantageUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        HotelAdvantage advantage=await _repository.GetByIdAsync(request.Id);
        if (advantage is null) throw new NotFoundException("Advantage not found");
        if (request.AdvantageName.IsNullOrEmpty()) throw new BadRequestException("Name cannot be null");
        HotelAdvantage existAct = await _repository.Table.FirstOrDefaultAsync(x => x.AdvantageName.ToLower() == request.AdvantageName.ToLower());
        if (existAct is not null && existAct.AdvantageName != advantage.AdvantageName && existAct.HotelId==advantage.HotelId)
            throw new BadRequestException("Advantage Name is already exist");
        advantage = _mapper.Map(request,advantage);
        advantage.ModifiedDate=DateTime.Now;
        await _repository.CommitAsync();
        return new AdvantageUpdateCommandResponse();
    }
}
