using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

public class ActivityGetByIdQueryHandler : IRequestHandler<ActivityGetByIdQueryRequest, ActivityGetByIdQueryResponse>
{
    private readonly IActivityRepository _repository;
    private readonly IMapper _mapper;

    public ActivityGetByIdQueryHandler(IActivityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ActivityGetByIdQueryResponse> Handle(ActivityGetByIdQueryRequest request, CancellationToken cancellationToken)
    {
        Activity activity = await _repository.GetByIdAsync(request.Id);
        if (activity is null) throw new Exception("Activity not found");
        ActivityGetByIdQueryResponse dto = _mapper.Map<ActivityGetByIdQueryResponse>(activity);
        return dto;
    }
}
