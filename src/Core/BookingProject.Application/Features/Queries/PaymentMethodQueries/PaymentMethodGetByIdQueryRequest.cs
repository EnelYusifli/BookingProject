using BookingProject.Application.Features.Queries.PaymentMethodQueries;
using MediatR;

public class PaymentMethodGetByIdQueryRequest : IRequest<PaymentMethodGetByIdQueryResponse>
{
    public int Id { get; set; }
}
