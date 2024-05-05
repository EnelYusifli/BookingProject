using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.ServiceCommands.ServiceDeleteCommands;

public class ServiceDeleteCommandHandler : IRequestHandler<ServiceDeleteCommandRequest, ServiceDeleteCommandResponse>
{
    private readonly IServiceRepository _repository;

    public ServiceDeleteCommandHandler(IServiceRepository repository)
    {
        _repository = repository;
    }
    public async Task<ServiceDeleteCommandResponse> Handle(ServiceDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        Service service = await _repository.GetByIdAsync(request.Id);
        if (service is null) throw new NotFoundException("Service not found");
        _repository.Delete(service);
        await _repository.CommitAsync();
        return new ServiceDeleteCommandResponse();
    }
}
