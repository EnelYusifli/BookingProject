using BookingProject.Application.Features.Queries.StaffLanguageQueries;
using MediatR;

public class StaffLanguageGetByIdQueryRequest : IRequest<StaffLanguageGetByIdQueryResponse>
{
    public int Id { get; set; }
}
