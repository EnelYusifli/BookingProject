using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelSoftDeleteCommands;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelApproveCommands;

public class HotelApproveCommandHandler : IRequestHandler<HotelApproveCommandRequest, HotelApproveCommandResponse>
{
	private readonly IHotelRepository _repository;
	private readonly IEmailService _emailService;
	private readonly IMediator _mediator;
	private readonly UserManager<AppUser> _userManager;

	public HotelApproveCommandHandler(IHotelRepository repository, IEmailService emailService,IMediator mediator,UserManager<AppUser> userManager)
    {
		_repository = repository;
		_emailService = emailService;
		_mediator = mediator;
		_userManager = userManager;
	}
    public async Task<HotelApproveCommandResponse> Handle(HotelApproveCommandRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			throw new NotFoundException("Request not found");

		Hotel hotel = await _repository.Table
		 .Include(x => x.AppUser)
		 .Include(x => x.HotelActivities)
			.Include(x => x.HotelAdvantages)
			.Include(x => x.HotelImages)
			.Include(x => x.HotelPaymentMethods)
			.Include(x => x.HotelServices)
			.Include(x => x.HotelStaffLanguages)
			.Include(x => x.Rooms)
			.ThenInclude(x => x.RoomImages)
			.Include(x => x.CustomerReviews)
			.ThenInclude(x => x.ReviewImages)
		 .FirstOrDefaultAsync(x => x.Id == request.Id);
		if (hotel.IsApproved == true && hotel.IsRefused == false)
			throw new BadRequestException("Hotel already approved");
		if (hotel is null)
			throw new NotFoundException($"Hotel with ID {request.Id} not found");
		var user = hotel.AppUser;
		if (user is null) throw new NotFoundException("No account found with this email address.");
		string subject = "Hotel Approval Notification";
		string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email", "requestapproved.html");
		string html = File.ReadAllText(filePath);
		html = html.Replace("{{hotelname}}", hotel.Name);
		html = html.Replace("{{username}}", user.FirstName+" "+user.LastName);
		hotel.ModifiedDate=DateTime.Now;
		hotel.IsApproved = true;
		hotel.IsRefused = false;
		HotelSoftDeleteCommandRequest activateRequest = new()
		{
			Id = request.Id
		};
		await _mediator.Send(activateRequest);
		await _userManager.AddToRoleAsync(user, "owner");
		await _emailService.SendEmail(user.Email, subject, html);
		return new HotelApproveCommandResponse();
	}
}
