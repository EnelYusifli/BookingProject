using AutoMapper;
using BookingProject.Application.Features.Queries.MessageQueries;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Queries.MessageQueries;

public class MessageGetAllQueryHandler : IRequestHandler<MessageGetAllQueryRequest,ICollection<MessageGetAllQueryResponse>>
{
	private readonly IMessageRepository _repository;
	private readonly IMapper _mapper;

	public MessageGetAllQueryHandler(IMessageRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}
	public async Task<ICollection<MessageGetAllQueryResponse>> Handle(MessageGetAllQueryRequest request, CancellationToken cancellationToken)
	{
		ICollection<Message> act = await _repository.GetAllAsync();
		if (act is null) throw new Exception("Message not found");
		ICollection<MessageGetAllQueryResponse> dtos = _mapper.Map<ICollection<MessageGetAllQueryResponse>>(act);
		return dtos;
	}
}
