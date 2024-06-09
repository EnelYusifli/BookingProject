using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Queries.TermsOfServiceQueries;

public class TermsOfServiceGetByIdQueryHandler : IRequestHandler<TermsOfServiceGetByIdQueryRequest, TermsOfServiceGetByIdQueryResponse>
{
	private readonly  ITermsOfServiceRepository _repository;
	private readonly IMapper _mapper;

	public TermsOfServiceGetByIdQueryHandler(ITermsOfServiceRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<TermsOfServiceGetByIdQueryResponse> Handle(TermsOfServiceGetByIdQueryRequest request, CancellationToken cancellationToken)
	{
		TermsOfService TermsOfService = await _repository.GetByIdAsync(2);
		if (TermsOfService is null) throw new Exception("TermsOfService not found");
		TermsOfServiceGetByIdQueryResponse dto = _mapper.Map<TermsOfServiceGetByIdQueryResponse>(TermsOfService);
		return dto;
	}
}
