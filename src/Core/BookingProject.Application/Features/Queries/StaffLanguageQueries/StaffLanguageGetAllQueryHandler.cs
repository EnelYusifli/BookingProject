using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using System.Reflection.Metadata;

namespace BookingProject.Application.Features.Queries.StaffLanguageQueries;

public class StaffLanguageGetAllQueryHandler : IRequestHandler<StaffLanguageGetAllQueryRequest, ICollection<StaffLanguageGetAllQueryResponse>>
{
    private readonly IStaffLanguageRepository _repository;
    private readonly IMapper _mapper;

    public StaffLanguageGetAllQueryHandler(IStaffLanguageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<StaffLanguageGetAllQueryResponse>> Handle(StaffLanguageGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        ICollection<StaffLanguage> act = await _repository.GetAllAsync();
        if (act is null) throw new Exception("StaffLanguage not found");
        ICollection<StaffLanguageGetAllQueryResponse> dtos = _mapper.Map<ICollection<StaffLanguageGetAllQueryResponse>>(act);
        return dtos;
    }
}
