using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.ServiceCommands.ServiceUpdateCommands;

public class ServiceUpdateCommandHandler : IRequestHandler<ServiceUpdateCommandRequest, ServiceUpdateCommandResponse>
{
    private readonly IServiceRepository _repository;
    private readonly IMapper _mapper;

    public ServiceUpdateCommandHandler(IServiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ServiceUpdateCommandResponse> Handle(ServiceUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        Service service=await _repository.GetByIdAsync(request.Id);
        if (service is null) throw new NotFoundException("Service not found");
        if (request.ServiceName.IsNullOrEmpty()) throw new BadRequestException("Name cannot be null");
        Service existAct = await _repository.Table.FirstOrDefaultAsync(x => x.ServiceName.ToLower() == request.ServiceName.ToLower());
        if (existAct is not null && existAct.ServiceName != service.ServiceName )
            throw new BadRequestException("Service Name is already exist");
        service = _mapper.Map(request,service);
        service.ModifiedDate=DateTime.Now;
        await _repository.CommitAsync();
        return new ServiceUpdateCommandResponse();
    }
}
