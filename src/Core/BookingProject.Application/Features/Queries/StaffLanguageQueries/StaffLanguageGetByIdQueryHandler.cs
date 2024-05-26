using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

public class StaffLanguageGetByIdQueryHandler : IRequestHandler<StaffLanguageGetByIdQueryRequest, StaffLanguageGetByIdQueryResponse>
{
    private readonly IStaffLanguageRepository _repository;
    private readonly IMapper _mapper;

    public StaffLanguageGetByIdQueryHandler(IStaffLanguageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StaffLanguageGetByIdQueryResponse> Handle(StaffLanguageGetByIdQueryRequest request, CancellationToken cancellationToken)
    {
        StaffLanguage StaffLanguage = await _repository.GetByIdAsync(request.Id);
        if (StaffLanguage is null) throw new Exception("StaffLanguage not found");
        StaffLanguageGetByIdQueryResponse dto = _mapper.Map<StaffLanguageGetByIdQueryResponse>(StaffLanguage);
        return dto;
    }
}
