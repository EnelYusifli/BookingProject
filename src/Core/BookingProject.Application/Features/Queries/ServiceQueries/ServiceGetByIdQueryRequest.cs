using BookingProject.Application.Features.Queries.ServiceQueries;
using MediatR;

public class ServiceGetByIdQueryRequest : IRequest<ServiceGetByIdQueryResponse>
{
    public int Id { get; set; }
}
