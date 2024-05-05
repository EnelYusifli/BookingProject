using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.TypeCommands.TypeSoftDeleteCommands;

public class TypeSoftDeleteCommandHandler : IRequestHandler<TypeSoftDeleteCommandRequest, TypeSoftDeleteCommandResponse>
{
    private readonly ITypeRepository _repository;
    private readonly IMapper _mapper;

    public TypeSoftDeleteCommandHandler(ITypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<TypeSoftDeleteCommandResponse> Handle(TypeSoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text=String.Empty;
        Domain.Entities.Type type = await _repository.GetByIdAsync(request.Id);
        if (type is null) throw new NotFoundException("Type not found");
        if (type.IsDeactive == true)
        {
            type.IsDeactive = false;
            text = "Type Activated";
        }
        else
        {
            type.IsDeactive = true;
            text = "Type Deactivated";
        }
        await _repository.CommitAsync();
        return new TypeSoftDeleteCommandResponse()
        {
            Text = text
        };

    }
}
