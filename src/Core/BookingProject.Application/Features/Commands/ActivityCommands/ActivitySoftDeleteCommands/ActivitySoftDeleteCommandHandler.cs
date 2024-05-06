using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.ActivityCommands.ActivitySoftDeleteCommands;

public class ActivitySoftDeleteCommandHandler : IRequestHandler<ActivitySoftDeleteCommandRequest, ActivitySoftDeleteCommandResponse>
{
    private readonly IActivityRepository _repository;
    private readonly IMapper _mapper;

    public ActivitySoftDeleteCommandHandler(IActivityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ActivitySoftDeleteCommandResponse> Handle(ActivitySoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text=String.Empty;
        Activity activity = await _repository.Table.Include(x=>x.HotelActivities).ThenInclude(x=>x.Hotel).FirstOrDefaultAsync(x=>x.Id==request.Id);
        if (activity is null) throw new NotFoundException("Activity not found");
        if (activity.IsDeactive == true)
        {
            activity.IsDeactive = false;
            text = "Activity Activated";
            foreach (var item in activity.HotelActivities)
            {
                if (item.Hotel.IsDeactive == false)
                    item.IsDeactive = false;
            }
        }
        else
        {
            activity.IsDeactive = true;
            text = "Activity Deactivated";
            foreach (var item in activity.HotelActivities)
            {
                item.IsDeactive = true;
            }
        }
        await _repository.CommitAsync();
        return new ActivitySoftDeleteCommandResponse()
        {
            Text = text
        };

    }
}
