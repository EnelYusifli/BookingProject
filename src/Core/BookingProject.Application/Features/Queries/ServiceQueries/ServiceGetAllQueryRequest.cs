using MediatR;

namespace BookingProject.Application.Features.Queries.ServiceQueries;

public class ServiceGetAllQueryRequest:IRequest<ICollection<ServiceGetAllQueryResponse>>
{
}
