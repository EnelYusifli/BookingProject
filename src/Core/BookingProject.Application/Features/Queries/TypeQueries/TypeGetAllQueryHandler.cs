using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using System.Reflection.Metadata;

namespace BookingProject.Application.Features.Queries.TypeQueries;

public class TypeGetAllQueryHandler : IRequestHandler<TypeGetAllQueryRequest, ICollection<TypeGetAllQueryResponse>>
{
    private readonly ITypeRepository _repository;
    private readonly IMapper _mapper;

    public TypeGetAllQueryHandler(ITypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<TypeGetAllQueryResponse>> Handle(TypeGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        ICollection<Domain.Entities.Type> act = await _repository.GetAllAsync();
        if (act is null) throw new Exception("Type not found");
        ICollection<TypeGetAllQueryResponse> dtos = _mapper.Map<ICollection<TypeGetAllQueryResponse>>(act);
        return dtos;
    }
}
