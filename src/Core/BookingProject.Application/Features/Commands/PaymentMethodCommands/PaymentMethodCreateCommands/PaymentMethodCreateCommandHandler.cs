using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodCreateCommands;

public class PaymentMethodCreateCommandHandler : IRequestHandler<PaymentMethodCreateCommandRequest, PaymentMethodCreateCommandResponse>
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IMapper _mapper;

    public PaymentMethodCreateCommandHandler(IPaymentMethodRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PaymentMethodCreateCommandResponse> Handle(PaymentMethodCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null)
        {
            throw new NotFoundException("Request not found");
        }
        if (request.PaymentMethodName.IsNullOrEmpty())
        {
            throw new BadRequestException("Name cannot be null");
        }
        if (await _repository.Table.AnyAsync(x => x.PaymentMethodName.ToLower() == request.PaymentMethodName.ToLower()))
            throw new BadRequestException("PaymentMethod Name is already exist");
        PaymentMethod paymentMethod=_mapper.Map<PaymentMethod>(request);
        await _repository.CreateAsync(paymentMethod);
        await _repository.CommitAsync();
        return new PaymentMethodCreateCommandResponse();
    }
}
