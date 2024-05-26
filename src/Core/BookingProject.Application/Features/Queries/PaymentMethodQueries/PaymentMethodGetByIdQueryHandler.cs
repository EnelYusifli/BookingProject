using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

public class PaymentMethodGetByIdQueryHandler : IRequestHandler<PaymentMethodGetByIdQueryRequest, PaymentMethodGetByIdQueryResponse>
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IMapper _mapper;

    public PaymentMethodGetByIdQueryHandler(IPaymentMethodRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaymentMethodGetByIdQueryResponse> Handle(PaymentMethodGetByIdQueryRequest request, CancellationToken cancellationToken)
    {
        PaymentMethod PaymentMethod = await _repository.GetByIdAsync(request.Id);
        if (PaymentMethod is null) throw new Exception("PaymentMethod not found");
        PaymentMethodGetByIdQueryResponse dto = _mapper.Map<PaymentMethodGetByIdQueryResponse>(PaymentMethod);
        return dto;
    }
}
