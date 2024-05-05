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
        ICollection<Activity> blogs = await _repository.GetAllAsync();
        if (blogs is null) throw new Exception("Blog not found");
        ICollection<ActivityGetAllQueryResponse> dtos = _mapper.Map<ICollection<ActivityGetAllQueryResponse>>(blogs);
        return dtos;
    }
}
