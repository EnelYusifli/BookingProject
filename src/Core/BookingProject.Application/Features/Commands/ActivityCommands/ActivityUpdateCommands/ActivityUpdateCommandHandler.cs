using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.ActivityCommands.ActivityUpdateCommands;

public class ActivityUpdateCommandHandler : IRequestHandler<ActivityUpdateCommandRequest, ActivityUpdateCommandResponse>
{
    private readonly IActivityRepository _repository;
    private readonly IMapper _mapper;

    public ActivityUpdateCommandHandler(IActivityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ActivityUpdateCommandResponse> Handle(ActivityUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        Activity activity=await _repository.GetByIdAsync(request.Id);
        if (activity is null) throw new NotFoundException("Activity not found");
        if (request.ActivityName.IsNullOrEmpty()) throw new BadRequestException("Name cannot be null");
        Activity existAct = await _repository.Table.FirstOrDefaultAsync(x => x.ActivityName.ToLower() == request.ActivityName.ToLower());
        if (existAct is not null && existAct.ActivityName != activity.ActivityName )
            throw new BadRequestException("Activity Name is already exist");
        activity = _mapper.Map(request,activity);
        activity.ModifiedDate=DateTime.Now;
        await _repository.CommitAsync();
        return new ActivityUpdateCommandResponse();
    }
}
