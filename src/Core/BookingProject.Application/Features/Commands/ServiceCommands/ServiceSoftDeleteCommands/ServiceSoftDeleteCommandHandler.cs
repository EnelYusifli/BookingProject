using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.ServiceCommands.ServiceSoftDeleteCommands;

public class ServiceSoftDeleteCommandHandler : IRequestHandler<ServiceSoftDeleteCommandRequest, ServiceSoftDeleteCommandResponse>
{
    private readonly IServiceRepository _repository;
    private readonly IMapper _mapper;

    public ServiceSoftDeleteCommandHandler(IServiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ServiceSoftDeleteCommandResponse> Handle(ServiceSoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text=String.Empty;
        Service service = await _repository.GetByIdAsync(request.Id);
        if (service is null) throw new NotFoundException("Service not found");
        if (service.IsDeactive == true)
        {
            service.IsDeactive = false;
            text = "Service Activated";
        }
        else
        {
            service.IsDeactive = true;
            text = "Service Deactivated";
        }
        await _repository.CommitAsync();
        return new ServiceSoftDeleteCommandResponse()
        {
            Text = text
        };

    }
}
