using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.ActivityCommands.ActivityDeleteCommands;

public class ActivityDeleteCommandHandler : IRequestHandler<ActivityDeleteCommandRequest, ActivityDeleteCommandResponse>
{
    private readonly IActivityRepository _repository;

    public ActivityDeleteCommandHandler(IActivityRepository repository)
    {
        _repository = repository;
    }
    public async Task<ActivityDeleteCommandResponse> Handle(ActivityDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        Activity activity = await _repository.GetByIdAsync(request.Id);
        if (activity is null) throw new NotFoundException("Activity not found");
        _repository.Delete(activity);
        await _repository.CommitAsync();
        return new ActivityDeleteCommandResponse();
    }
}
