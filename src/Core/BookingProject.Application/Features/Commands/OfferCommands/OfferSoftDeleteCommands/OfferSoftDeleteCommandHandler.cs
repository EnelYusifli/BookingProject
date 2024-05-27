using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.OfferCommands.OfferSoftDeleteCommands;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.OfferCommands.OfferSoftDeleteCommands;

public class OfferSoftDeleteCommandHandler : IRequestHandler<OfferSoftDeleteCommandRequest, OfferSoftDeleteCommandResponse>
{
	private readonly IOfferRepository _repository;
	private readonly IMapper _mapper;

	public OfferSoftDeleteCommandHandler(IOfferRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}
	public async Task<OfferSoftDeleteCommandResponse> Handle(OfferSoftDeleteCommandRequest request, CancellationToken cancellationToken)
	{
		string text = String.Empty;
		Offer Offer = await _repository.Table.FirstOrDefaultAsync(x => x.Id == request.Id);
		if (Offer is null) throw new NotFoundException("Offer not found");
		if (Offer.IsDeactive == true)
		{
			Offer.IsDeactive = false;
			text = "Offer Activated";
		}
		else
		{
			Offer.IsDeactive = true;
			text = "Offer Deactivated";
		}
		await _repository.CommitAsync();
		return new OfferSoftDeleteCommandResponse()
		{
			Text = text
		};

	}
}
