using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageCreateCommands;

public class StaffLanguageCreateCommandHandler : IRequestHandler<StaffLanguageCreateCommandRequest, StaffLanguageCreateCommandResponse>
{
    private readonly IStaffLanguageRepository _repository;
    private readonly IMapper _mapper;

    public StaffLanguageCreateCommandHandler(IStaffLanguageRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<StaffLanguageCreateCommandResponse> Handle(StaffLanguageCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null)
        {
            throw new NotFoundException("Request not found");
        }
        if (request.StaffLanguageName.IsNullOrEmpty())
        {
            throw new BadRequestException("Name cannot be null");
        }
        if (await _repository.Table.AnyAsync(x => x.StaffLanguageName.ToLower() == request.StaffLanguageName.ToLower()))
            throw new BadRequestException("StaffLanguage Name is already exist");
        StaffLanguage staffLanguage=_mapper.Map<StaffLanguage>(request);
        await _repository.CreateAsync(staffLanguage);
        await _repository.CommitAsync();
        return new StaffLanguageCreateCommandResponse();
    }
}
