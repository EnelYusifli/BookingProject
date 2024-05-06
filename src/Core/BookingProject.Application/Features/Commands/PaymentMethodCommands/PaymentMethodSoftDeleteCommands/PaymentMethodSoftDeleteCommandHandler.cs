using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        PaymentMethod paymentMethod = await _repository.Table.Include(x=>x.HotelPaymentMethods).ThenInclude(x=>x.Hotel).FirstOrDefaultAsync(x=>x.Id==request.Id);
        if (paymentMethod is null) throw new NotFoundException("PaymentMethod not found");
        if (paymentMethod.IsDeactive == true)
        {
            paymentMethod.IsDeactive = false;
            text = "PaymentMethod Activated";
            foreach (var item in paymentMethod.HotelPaymentMethods)
            {
                if (item.Hotel.IsDeactive == false)
                    item.IsDeactive = false;
            }
        }
        else
        {
            paymentMethod.IsDeactive = true;
            text = "PaymentMethod Deactivated";
            foreach (var item in paymentMethod.HotelPaymentMethods)
            {
                    item.IsDeactive = false;
            }
        }
        await _repository.CommitAsync();
        return new PaymentMethodSoftDeleteCommandResponse()
        {
            Text = text
        };

    }
}
