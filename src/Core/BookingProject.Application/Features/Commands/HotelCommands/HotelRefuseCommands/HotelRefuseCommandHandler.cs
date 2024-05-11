using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.HotelCommands.HotelRefuseCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelSoftDeleteCommands;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelRefuseCommands;

public class HotelRefuseCommandHandler : IRequestHandler<HotelRefuseCommandRequest, HotelRefuseCommandResponse>
{
	private readonly IHotelRepository _repository;
	private readonly IEmailService _emailService;

	public HotelRefuseCommandHandler(IHotelRepository repository, IEmailService emailService, IMediator mediator)
	{
		_repository = repository;
		_emailService = emailService;
	}
	public async Task<HotelRefuseCommandResponse> Handle(HotelRefuseCommandRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			throw new NotFoundException("Request not found");

		Hotel hotel = await _repository.Table
			.Include(x=>x.AppUser)
		 .FirstOrDefaultAsync(x => x.Id == request.Id);
		if (hotel is null)
			throw new NotFoundException($"Hotel with ID {request.Id} not found");
		if (hotel.IsApproved == false && hotel.IsRefused == true)
			throw new BadRequestException("Hotel already refused");
		if (hotel.IsApproved == true && hotel.IsRefused == false)
			throw new BadRequestException("Hotel already approved");
		var user = hotel.AppUser;
		if (user is null) throw new NotFoundException("No account found with this email address.");
		string subject = "Hotel Refusal Notification";
		string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email", "requestrefused.html");
		string html = File.ReadAllText(filePath);
		html = html.Replace("{{hotelname}}", hotel.Name);
		html = html.Replace("{{username}}", user.FirstName + " " + user.LastName);
		hotel.ModifiedDate = DateTime.Now;
		hotel.IsApproved = false;
		hotel.IsRefused = true;
		hotel.IsDeactive = true;
		await _repository.CommitAsync();
		await _emailService.SendEmail(user.Email, subject, html);
		return new HotelRefuseCommandResponse();
	}
}
