using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;

public class ActivityCreateCommandHandler : IRequestHandler<ActivityCreateCommandRequest, ActivityCreateCommandResponse>
{
    private readonly IActivityRepository _repository;
    private readonly IMapper _mapper;

    public ActivityCreateCommandHandler(IActivityRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ActivityCreateCommandResponse> Handle(ActivityCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null)
        {
            throw new NotFoundException("Request not found");
        }
        if (request.ActivityName.IsNullOrEmpty())
        {
            throw new BadRequestException("Name cannot be null");
        }
        Activity activity=_mapper.Map<Activity>(request);
        await _repository.CreateAsync(activity);
        await _repository.CommitAsync();
        return new ActivityCreateCommandResponse();
    }
}
