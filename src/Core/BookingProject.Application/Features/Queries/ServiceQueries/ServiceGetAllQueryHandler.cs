using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using System.Reflection.Metadata;

namespace BookingProject.Application.Features.Queries.ServiceQueries;

public class ServiceGetAllQueryHandler : IRequestHandler<ServiceGetAllQueryRequest, ICollection<ServiceGetAllQueryResponse>>
{
    private readonly IServiceRepository _repository;
    private readonly IMapper _mapper;

    public ServiceGetAllQueryHandler(IServiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<ServiceGetAllQueryResponse>> Handle(ServiceGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        ICollection<Service> act = await _repository.GetAllAsync();
        if (act is null) throw new Exception("Service not found");
        ICollection<ServiceGetAllQueryResponse> dtos = _mapper.Map<ICollection<ServiceGetAllQueryResponse>>(act);
        return dtos;
    }
}
