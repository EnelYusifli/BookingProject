using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.TypeCommands.TypeDeleteCommands;

public class TypeDeleteCommandHandler : IRequestHandler<TypeDeleteCommandRequest, TypeDeleteCommandResponse>
{
    private readonly ITypeRepository _repository;

    public TypeDeleteCommandHandler(ITypeRepository repository)
    {
        _repository = repository;
    }
    public async Task<TypeDeleteCommandResponse> Handle(TypeDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Type type = await _repository.GetByIdAsync(request.Id);
        if (type is null) throw new NotFoundException("Type not found");
        _repository.Delete(type);
        await _repository.CommitAsync();
        return new TypeDeleteCommandResponse();
    }
}
