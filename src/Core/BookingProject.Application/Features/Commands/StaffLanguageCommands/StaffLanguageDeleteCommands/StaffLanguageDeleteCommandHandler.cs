using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageDeleteCommands;

public class StaffLanguageDeleteCommandHandler : IRequestHandler<StaffLanguageDeleteCommandRequest, StaffLanguageDeleteCommandResponse>
{
    private readonly IStaffLanguageRepository _repository;

    public StaffLanguageDeleteCommandHandler(IStaffLanguageRepository repository)
    {
        _repository = repository;
    }
    public async Task<StaffLanguageDeleteCommandResponse> Handle(StaffLanguageDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        StaffLanguage staffLanguage = await _repository.GetByIdAsync(request.Id);
        if (staffLanguage is null) throw new NotFoundException("StaffLanguage not found");
        _repository.Delete(staffLanguage);
        await _repository.CommitAsync();
        return new StaffLanguageDeleteCommandResponse();
    }
}
