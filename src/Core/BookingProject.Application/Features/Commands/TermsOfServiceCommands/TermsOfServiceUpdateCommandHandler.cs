using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookingProject.Application.Features.Commands.TermsOfServiceCommands.TermsOfServiceUpdateCommands;

public class TermsOfServiceUpdateCommandHandler : IRequestHandler<TermsOfServiceUpdateCommandRequest, TermsOfServiceUpdateCommandResponse>
{
	private readonly ITermsOfServiceRepository _repository;
	private readonly IMapper _mapper;

	public TermsOfServiceUpdateCommandHandler(ITermsOfServiceRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<TermsOfServiceUpdateCommandResponse> Handle(TermsOfServiceUpdateCommandRequest request, CancellationToken cancellationToken)
	{
		TermsOfService termsOfService = await _repository.GetByIdAsync(2);
		if (termsOfService is null) throw new NotFoundException("TermsOfService not found");
		if (request.Title.IsNullOrEmpty()) throw new BadRequestException("Title cannot be null");
		if (request.Text.IsNullOrEmpty()) throw new BadRequestException("Text cannot be null");
		termsOfService = _mapper.Map(request, termsOfService);
		await _repository.CommitAsync();

		return new TermsOfServiceUpdateCommandResponse();
	}
}
