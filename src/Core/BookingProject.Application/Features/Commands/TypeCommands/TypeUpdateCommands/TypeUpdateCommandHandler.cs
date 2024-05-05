using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.TypeCommands.TypeUpdateCommands;

public class TypeUpdateCommandHandler : IRequestHandler<TypeUpdateCommandRequest, TypeUpdateCommandResponse>
{
    private readonly ITypeRepository _repository;
    private readonly IMapper _mapper;

    public TypeUpdateCommandHandler(ITypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<TypeUpdateCommandResponse> Handle(TypeUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Type type=await _repository.GetByIdAsync(request.Id);
        if (type is null) throw new NotFoundException("Type not found");
        if (request.TypeName.IsNullOrEmpty()) throw new BadRequestException("Name cannot be null");
        Domain.Entities.Type existAct = await _repository.Table.FirstOrDefaultAsync(x => x.TypeName.ToLower() == request.TypeName.ToLower());
        if (existAct is not null && existAct.TypeName != type.TypeName )
            throw new BadRequestException("Type Name is already exist");
        type = _mapper.Map(request,type);
        type.ModifiedDate=DateTime.Now;
        await _repository.CommitAsync();
        return new TypeUpdateCommandResponse();
    }
}
