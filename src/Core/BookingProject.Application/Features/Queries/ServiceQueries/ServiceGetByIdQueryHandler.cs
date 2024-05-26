using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

public class ServiceGetByIdQueryHandler : IRequestHandler<ServiceGetByIdQueryRequest, ServiceGetByIdQueryResponse>
{
    private readonly IServiceRepository _repository;
    private readonly IMapper _mapper;

    public ServiceGetByIdQueryHandler(IServiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceGetByIdQueryResponse> Handle(ServiceGetByIdQueryRequest request, CancellationToken cancellationToken)
    {
        Service Service = await _repository.GetByIdAsync(request.Id);
        if (Service is null) throw new Exception("Service not found");
        ServiceGetByIdQueryResponse dto = _mapper.Map<ServiceGetByIdQueryResponse>(Service);
        return dto;
    }
}
