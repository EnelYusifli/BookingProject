using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageSoftDeleteCommands;

public class AdvantageSoftDeleteCommandHandler : IRequestHandler<AdvantageSoftDeleteCommandRequest, AdvantageSoftDeleteCommandResponse>
{
    private readonly IAdvantageRepository _repository;
    private readonly IMapper _mapper;

    public AdvantageSoftDeleteCommandHandler(IAdvantageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<AdvantageSoftDeleteCommandResponse> Handle(AdvantageSoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text=String.Empty;
        HotelAdvantage advantage = await _repository.GetByIdAsync(request.Id);
        if (advantage is null) throw new NotFoundException("Advantage not found");
        if (advantage.IsDeactive == true)
        {
            advantage.IsDeactive = false;
            text = "Advantage Activated";
        }
        else
        {
            advantage.IsDeactive = true;
            text = "Advantage Deactivated";
        }
        await _repository.CommitAsync();
        return new AdvantageSoftDeleteCommandResponse()
        {
            Text = text
        };

    }
}
