using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.MessageCommands.MessageCreateCommands;

public class MessageCreateCommandHandler : IRequestHandler<MessageCreateCommandRequest, MessageCreateCommandResponse>
{
	private readonly IMessageRepository _repository;
	private readonly IMapper _mapper;

	public MessageCreateCommandHandler(IMessageRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}
	public async Task<MessageCreateCommandResponse> Handle(MessageCreateCommandRequest request, CancellationToken cancellationToken)
	{
		if (request == null) throw new NotFoundException("Request not found");
		var message = _mapper.Map<Message>(request);
		await _repository.CreateAsync(message);
		await _repository.CommitAsync();
		return new MessageCreateCommandResponse();
	}
}
