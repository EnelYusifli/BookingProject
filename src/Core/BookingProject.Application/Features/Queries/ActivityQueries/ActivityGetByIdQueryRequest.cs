using BookingProject.Application.Features.Queries.ActivityQueries;
using MediatR;

public class ActivityGetByIdQueryRequest : IRequest<ActivityGetByIdQueryResponse>
{
    public int Id { get; set; }
}
