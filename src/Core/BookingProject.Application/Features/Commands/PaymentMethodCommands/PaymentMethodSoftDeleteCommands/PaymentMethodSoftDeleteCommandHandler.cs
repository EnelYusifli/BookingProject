using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodSoftDeleteCommands;

public class PaymentMethodSoftDeleteCommandHandler : IRequestHandler<PaymentMethodSoftDeleteCommandRequest, PaymentMethodSoftDeleteCommandResponse>
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IMapper _mapper;

    public PaymentMethodSoftDeleteCommandHandler(IPaymentMethodRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PaymentMethodSoftDeleteCommandResponse> Handle(PaymentMethodSoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text=String.Empty;
        PaymentMethod paymentMethod = await _repository.GetByIdAsync(request.Id);
        if (paymentMethod is null) throw new NotFoundException("PaymentMethod not found");
        if (paymentMethod.IsDeactive == true)
        {
            paymentMethod.IsDeactive = false;
            text = "PaymentMethod Activated";
        }
        else
        {
            paymentMethod.IsDeactive = true;
            text = "PaymentMethod Deactivated";
        }
        await _repository.CommitAsync();
        return new PaymentMethodSoftDeleteCommandResponse()
        {
            Text = text
        };

    }
}
