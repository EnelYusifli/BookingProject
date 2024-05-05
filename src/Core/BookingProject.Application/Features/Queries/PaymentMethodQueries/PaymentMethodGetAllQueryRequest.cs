using MediatR;

namespace BookingProject.Application.Features.Queries.PaymentMethodQueries;

public class PaymentMethodGetAllQueryRequest:IRequest<ICollection<PaymentMethodGetAllQueryResponse>>
{
}
