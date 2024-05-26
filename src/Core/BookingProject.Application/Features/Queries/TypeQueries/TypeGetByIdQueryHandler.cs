using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

public class TypeGetByIdQueryHandler : IRequestHandler<TypeGetByIdQueryRequest, TypeGetByIdQueryResponse>
{
    private readonly ITypeRepository _repository;
    private readonly IMapper _mapper;

    public TypeGetByIdQueryHandler(ITypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TypeGetByIdQueryResponse> Handle(TypeGetByIdQueryRequest request, CancellationToken cancellationToken)
    {
        BookingProject.Domain.Entities.Type Type = await _repository.GetByIdAsync(request.Id);
        if (Type is null) throw new Exception("Type not found");
        TypeGetByIdQueryResponse dto = _mapper.Map<TypeGetByIdQueryResponse>(Type);
        return dto;
    }
}
