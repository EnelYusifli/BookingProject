using MediatR;

namespace BookingProject.Application.Features.Queries.StaffLanguageQueries;

public class StaffLanguageGetAllQueryRequest:IRequest<ICollection<StaffLanguageGetAllQueryResponse>>
{
}
