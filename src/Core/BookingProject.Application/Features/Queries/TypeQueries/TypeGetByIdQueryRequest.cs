using BookingProject.Application.Features.Queries.TypeQueries;
using MediatR;

public class TypeGetByIdQueryRequest : IRequest<TypeGetByIdQueryResponse>
{
    public int Id { get; set; }
}
