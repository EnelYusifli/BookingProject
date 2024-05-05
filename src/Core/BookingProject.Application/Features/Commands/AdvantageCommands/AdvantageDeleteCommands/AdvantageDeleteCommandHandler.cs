using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageDeleteCommands;

public class AdvantageDeleteCommandHandler : IRequestHandler<AdvantageDeleteCommandRequest, AdvantageDeleteCommandResponse>
{
    private readonly IAdvantageRepository _repository;

    public AdvantageDeleteCommandHandler(IAdvantageRepository repository)
    {
        _repository = repository;
    }
    public async Task<AdvantageDeleteCommandResponse> Handle(AdvantageDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        HotelAdvantage advantage = await _repository.GetByIdAsync(request.Id);
        if (advantage is null) throw new NotFoundException("Advantage not found");
        _repository.Delete(advantage);
        await _repository.CommitAsync();
        return new AdvantageDeleteCommandResponse();
    }
}
