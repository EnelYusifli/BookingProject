using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.TypeCommands.TypeCreateCommands;

public class TypeCreateCommandHandler : IRequestHandler<TypeCreateCommandRequest, TypeCreateCommandResponse>
{
    private readonly ITypeRepository _repository;
    private readonly IMapper _mapper;

    public TypeCreateCommandHandler(ITypeRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<TypeCreateCommandResponse> Handle(TypeCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null)
        {
            throw new NotFoundException("Request not found");
        }
        if (request.TypeName.IsNullOrEmpty())
        {
            throw new BadRequestException("Name cannot be null");
        }
        if (await _repository.Table.AnyAsync(x => x.TypeName.ToLower() == request.TypeName.ToLower()))
            throw new BadRequestException("Type Name is already exist");
        Domain.Entities.Type type=_mapper.Map<Domain.Entities.Type>(request);
        await _repository.CreateAsync(type);
        await _repository.CommitAsync();
        return new TypeCreateCommandResponse();
    }
}
