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

namespace BookingProject.Application.Features.Commands.AboutCommands.AboutUpdateCommands;

public class AboutUpdateCommandHandler : IRequestHandler<AboutUpdateCommandRequest, AboutUpdateCommandResponse>
{
	private readonly IAboutRepository _repository;
	private readonly IMapper _mapper;

	public AboutUpdateCommandHandler(IAboutRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<AboutUpdateCommandResponse> Handle(AboutUpdateCommandRequest request, CancellationToken cancellationToken)
	{
		About about = await _repository.GetByIdAsync(1);
		if (about is null) throw new NotFoundException("About not found");
		if (request.StoryTitle.IsNullOrEmpty()) throw new BadRequestException("Story title cannot be null");
		if (request.Story.IsNullOrEmpty()) throw new BadRequestException("Story cannot be null");
		about = _mapper.Map(request, about);
		await _repository.CommitAsync();

		return new AboutUpdateCommandResponse();
	}
}
