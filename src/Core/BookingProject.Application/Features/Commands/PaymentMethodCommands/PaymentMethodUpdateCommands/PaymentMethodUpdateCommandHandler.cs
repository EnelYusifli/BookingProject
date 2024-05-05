using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodUpdateCommands;

public class PaymentMethodUpdateCommandHandler : IRequestHandler<PaymentMethodUpdateCommandRequest, PaymentMethodUpdateCommandResponse>
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IMapper _mapper;

    public PaymentMethodUpdateCommandHandler(IPaymentMethodRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PaymentMethodUpdateCommandResponse> Handle(PaymentMethodUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        PaymentMethod paymentMethod=await _repository.GetByIdAsync(request.Id);
        if (paymentMethod is null) throw new NotFoundException("PaymentMethod not found");
        if (request.PaymentMethodName.IsNullOrEmpty()) throw new BadRequestException("Name cannot be null");
        PaymentMethod existAct = await _repository.Table.FirstOrDefaultAsync(x => x.PaymentMethodName.ToLower() == request.PaymentMethodName.ToLower());
        if (existAct is not null && existAct.PaymentMethodName != paymentMethod.PaymentMethodName )
            throw new BadRequestException("PaymentMethod Name is already exist");
        paymentMethod = _mapper.Map(request,paymentMethod);
        paymentMethod.ModifiedDate=DateTime.Now;
        await _repository.CommitAsync();
        return new PaymentMethodUpdateCommandResponse();
    }
}
