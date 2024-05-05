using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using System.Reflection.Metadata;

namespace BookingProject.Application.Features.Queries.PaymentMethodQueries;

public class PaymentMethodGetAllQueryHandler : IRequestHandler<PaymentMethodGetAllQueryRequest, ICollection<PaymentMethodGetAllQueryResponse>>
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IMapper _mapper;

    public PaymentMethodGetAllQueryHandler(IPaymentMethodRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<PaymentMethodGetAllQueryResponse>> Handle(PaymentMethodGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        ICollection<PaymentMethod> act = await _repository.GetAllAsync();
        if (act is null) throw new Exception("PaymentMethod not found");
        ICollection<PaymentMethodGetAllQueryResponse> dtos = _mapper.Map<ICollection<PaymentMethodGetAllQueryResponse>>(act);
        return dtos;
    }
}
