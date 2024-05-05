using MediatR;

namespace BookingProject.Application.Features.Queries.TypeQueries;

public class TypeGetAllQueryRequest:IRequest<ICollection<TypeGetAllQueryResponse>>
{
}
