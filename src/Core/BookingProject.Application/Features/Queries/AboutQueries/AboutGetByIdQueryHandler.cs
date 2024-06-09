using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Queries.AboutQueries;

public class AboutGetByIdQueryHandler : IRequestHandler<AboutGetByIdQueryRequest, AboutGetByIdQueryResponse>
{
	private readonly IAboutRepository _repository;
	private readonly IMapper _mapper;

	public AboutGetByIdQueryHandler(IAboutRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<AboutGetByIdQueryResponse> Handle(AboutGetByIdQueryRequest request, CancellationToken cancellationToken)
	{
		About about = await _repository.GetByIdAsync(1);
		if (about is null) throw new Exception("About not found");
		AboutGetByIdQueryResponse dto = _mapper.Map<AboutGetByIdQueryResponse>(about);
		return dto;
	}
}
