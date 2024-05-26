using BookingProject.Application.Features.Queries.CountryQueries;
using MediatR;

public class CountryGetByIdQueryRequest : IRequest<CountryGetByIdQueryResponse>
{
    public int Id { get; set; }
}
