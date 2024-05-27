using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.DiscountCommands.DiscountSoftDeleteCommands;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.DiscountCommands.DiscountSoftDeleteCommands;

public class DiscountSoftDeleteCommandHandler : IRequestHandler<DiscountSoftDeleteCommandRequest, DiscountSoftDeleteCommandResponse>
{
	private readonly IDiscountRepository _repository;
	private readonly IMapper _mapper;

	public DiscountSoftDeleteCommandHandler(IDiscountRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}
	public async Task<DiscountSoftDeleteCommandResponse> Handle(DiscountSoftDeleteCommandRequest request, CancellationToken cancellationToken)
	{
		string text = String.Empty;
		Discount discount = await _repository.Table.FirstOrDefaultAsync(x => x.Id == request.Id);
		if (discount is null) throw new NotFoundException("Discount not found");
		if (discount.IsDeactive == true)
		{
			discount.IsDeactive = false;
			text = "Discount Activated";
		}
		else
		{
			discount.IsDeactive = true;
			text = "Discount Deactivated";
		}
		await _repository.CommitAsync();
		return new DiscountSoftDeleteCommandResponse()
		{
			Text = text
		};

	}
}
