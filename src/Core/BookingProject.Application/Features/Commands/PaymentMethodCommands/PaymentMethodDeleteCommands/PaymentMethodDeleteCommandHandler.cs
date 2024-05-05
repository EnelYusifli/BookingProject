using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodDeleteCommands;

public class PaymentMethodDeleteCommandHandler : IRequestHandler<PaymentMethodDeleteCommandRequest, PaymentMethodDeleteCommandResponse>
{
    private readonly IPaymentMethodRepository _repository;

    public PaymentMethodDeleteCommandHandler(IPaymentMethodRepository repository)
    {
        _repository = repository;
    }
    public async Task<PaymentMethodDeleteCommandResponse> Handle(PaymentMethodDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        PaymentMethod paymentMethod = await _repository.GetByIdAsync(request.Id);
        if (paymentMethod is null) throw new NotFoundException("PaymentMethod not found");
        _repository.Delete(paymentMethod);
        await _repository.CommitAsync();
        return new PaymentMethodDeleteCommandResponse();
    }
}
