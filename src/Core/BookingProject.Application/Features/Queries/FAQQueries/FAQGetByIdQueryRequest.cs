using BookingProject.Application.Features.Queries.FAQQueries;
using MediatR;

public class FAQGetByIdQueryRequest : IRequest<FAQGetByIdQueryResponse>
{
    public int Id { get; set; }
}
