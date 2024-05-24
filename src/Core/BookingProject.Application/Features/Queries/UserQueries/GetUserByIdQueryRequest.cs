using MediatR;

namespace BookingProject.Application.Features.Queries.UserQueries;

public class GetUserByIdQueryRequest:IRequest<GetUserByIdQueryResponse>
{
    public string Id { get; set; }
}
