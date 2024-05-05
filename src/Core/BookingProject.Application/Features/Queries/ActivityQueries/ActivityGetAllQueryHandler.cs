using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using System.Reflection.Metadata;

namespace BookingProject.Application.Features.Queries.ActivityQueries;

public class ActivityGetAllQueryHandler : IRequestHandler<ActivityGetAllQueryRequest, ICollection<ActivityGetAllQueryResponse>>
{
    private readonly IActivityRepository _repository;
    private readonly IMapper _mapper;

    public ActivityGetAllQueryHandler(IActivityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<ActivityGetAllQueryResponse>> Handle(ActivityGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        ICollection<Activity> act = await _repository.GetAllAsync();
        if (act is null) throw new Exception("Activity not found");
        ICollection<ActivityGetAllQueryResponse> dtos = _mapper.Map<ICollection<ActivityGetAllQueryResponse>>(act);
        return dtos;
    }
}
