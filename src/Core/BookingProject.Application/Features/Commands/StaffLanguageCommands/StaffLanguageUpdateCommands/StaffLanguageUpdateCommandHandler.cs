using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageUpdateCommands;

public class StaffLanguageUpdateCommandHandler : IRequestHandler<StaffLanguageUpdateCommandRequest, StaffLanguageUpdateCommandResponse>
{
    private readonly IStaffLanguageRepository _repository;
    private readonly IMapper _mapper;

    public StaffLanguageUpdateCommandHandler(IStaffLanguageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<StaffLanguageUpdateCommandResponse> Handle(StaffLanguageUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        StaffLanguage staffLanguage=await _repository.GetByIdAsync(request.Id);
        if (staffLanguage is null) throw new NotFoundException("StaffLanguage not found");
        if (request.StaffLanguageName.IsNullOrEmpty()) throw new BadRequestException("Name cannot be null");
        StaffLanguage existAct = await _repository.Table.FirstOrDefaultAsync(x => x.StaffLanguageName.ToLower() == request.StaffLanguageName.ToLower());
        if (existAct is not null && existAct.StaffLanguageName != staffLanguage.StaffLanguageName )
            throw new BadRequestException("StaffLanguage Name is already exist");
        staffLanguage = _mapper.Map(request,staffLanguage);
        staffLanguage.ModifiedDate=DateTime.Now;
        await _repository.CommitAsync();
        return new StaffLanguageUpdateCommandResponse();
    }
}
