using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

public class FAQGetByIdQueryHandler : IRequestHandler<FAQGetByIdQueryRequest, FAQGetByIdQueryResponse>
{
    private readonly IFAQsRepository _repository;
    private readonly IMapper _mapper;

    public FAQGetByIdQueryHandler(IFAQsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FAQGetByIdQueryResponse> Handle(FAQGetByIdQueryRequest request, CancellationToken cancellationToken)
    {
        FAQ FAQ = await _repository.GetByIdAsync(request.Id);
        if (FAQ is null) throw new Exception("FAQ not found");
        FAQGetByIdQueryResponse dto = _mapper.Map<FAQGetByIdQueryResponse>(FAQ);
        return dto;
    }
}
