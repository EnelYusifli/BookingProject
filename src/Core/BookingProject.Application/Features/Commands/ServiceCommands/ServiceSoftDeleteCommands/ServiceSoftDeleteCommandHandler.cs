using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        Service service =await _repository.Table.Include(x => x.HotelServices).ThenInclude(x=>x.Hotel).FirstOrDefaultAsync(x => x.Id == request.Id);
        if (service is null) throw new NotFoundException("Service not found");
        if (service.IsDeactive == true)
        {
            service.IsDeactive = false;
            text = "Service Activated";
            foreach (var item in service.HotelServices)
            {
                if (item.Hotel.IsDeactive == false)
                    item.IsDeactive = false;
            }
        }
        else
        {
            service.IsDeactive = true;
            text = "Service Deactivated";
            foreach (var item in service.HotelServices)
            {
                    item.IsDeactive = true;
            }
        }
        await _repository.CommitAsync();
        return new ServiceSoftDeleteCommandResponse()
        {
            Text = text
        };

    }
}
