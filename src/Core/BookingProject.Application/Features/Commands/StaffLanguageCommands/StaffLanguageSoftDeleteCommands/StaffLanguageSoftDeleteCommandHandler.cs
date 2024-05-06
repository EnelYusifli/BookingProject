using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageSoftDeleteCommands;

public class StaffLanguageSoftDeleteCommandHandler : IRequestHandler<StaffLanguageSoftDeleteCommandRequest, StaffLanguageSoftDeleteCommandResponse>
{
    private readonly IStaffLanguageRepository _repository;
    private readonly IMapper _mapper;

    public StaffLanguageSoftDeleteCommandHandler(IStaffLanguageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<StaffLanguageSoftDeleteCommandResponse> Handle(StaffLanguageSoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        string text=String.Empty;
        StaffLanguage staffLanguage = await _repository.Table.Include(x => x.HotelStaffLanguages).ThenInclude(x => x.Hotel).FirstOrDefaultAsync(x => x.Id == request.Id);
        if (staffLanguage is null) throw new NotFoundException("StaffLanguage not found");
        if (staffLanguage.IsDeactive == true)
        {
            staffLanguage.IsDeactive = false;
            text = "StaffLanguage Activated";
            foreach (var item in staffLanguage.HotelStaffLanguages)
            {
                if (item.Hotel.IsDeactive == false)
                    item.IsDeactive = false;
            }
        }
        else
        {
            staffLanguage.IsDeactive = true;
            text = "StaffLanguage Deactivated";
            foreach (var item in staffLanguage.HotelStaffLanguages)
            {
                    item.IsDeactive = true;
            }
        }
        await _repository.CommitAsync();
        return new StaffLanguageSoftDeleteCommandResponse()
        {
            Text = text
        };

    }
}
