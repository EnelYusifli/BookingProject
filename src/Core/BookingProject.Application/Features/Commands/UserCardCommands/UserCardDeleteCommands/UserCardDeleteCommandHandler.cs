using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.UserCardCommands.UserCardDeleteCommands;

public class UserCardDeleteCommandHandler : IRequestHandler<UserCardDeleteCommandRequest, UserCardDeleteCommandResponse>
{
	private readonly ICardRepository _repository;

	public UserCardDeleteCommandHandler(ICardRepository repository)
    {
		_repository = repository;
	}
    public async Task<UserCardDeleteCommandResponse> Handle(UserCardDeleteCommandRequest request, CancellationToken cancellationToken)
	{
		UserCard card = await _repository.Table.FirstOrDefaultAsync(x => x.Id == request.Id);
		if (card is null) throw new NotFoundException("Card not found");
		card.IsDeactive = true;
		await _repository.CommitAsync();
		return new UserCardDeleteCommandResponse();
	}
}
