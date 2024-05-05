using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.ServiceCommands.ServiceCreateCommands;

public class ServiceCreateCommandHandler : IRequestHandler<ServiceCreateCommandRequest, ServiceCreateCommandResponse>
{
    private readonly IServiceRepository _repository;
    private readonly IMapper _mapper;

    public ServiceCreateCommandHandler(IServiceRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ServiceCreateCommandResponse> Handle(ServiceCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null)
        {
            throw new NotFoundException("Request not found");
        }
        if (request.ServiceName.IsNullOrEmpty())
        {
            throw new BadRequestException("Name cannot be null");
        }
        if (await _repository.Table.AnyAsync(x => x.ServiceName.ToLower() == request.ServiceName.ToLower()))
            throw new BadRequestException("Service Name is already exist");
        Service service=_mapper.Map<Service>(request);
        await _repository.CreateAsync(service);
        await _repository.CommitAsync();
        return new ServiceCreateCommandResponse();
    }
}
