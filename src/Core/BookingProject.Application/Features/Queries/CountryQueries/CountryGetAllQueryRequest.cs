using MediatR;

namespace BookingProject.Application.Features.Queries.CountryQueries;

public class CountryGetAllQueryRequest:IRequest<ICollection<CountryGetAllQueryResponse>>
{
}
