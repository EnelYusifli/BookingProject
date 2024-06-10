using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.MessageCommands.MessageDeleteCommands;

public class MessageDeleteCommandHandler : IRequestHandler<MessageDeleteCommandRequest, MessageDeleteCommandResponse>
{
    private readonly IMessageRepository _repository;

    public MessageDeleteCommandHandler(IMessageRepository repository)
    {
        _repository = repository;
    }
    public async Task<MessageDeleteCommandResponse> Handle(MessageDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        Message Message = await _repository.GetByIdAsync(request.Id);
        if (Message is null) throw new NotFoundException("Message not found");
        _repository.Delete(Message);
        await _repository.CommitAsync();
        return new MessageDeleteCommandResponse();
    }
}
