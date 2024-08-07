﻿using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmtpServer.Text;

namespace BookingProject.Application.Features.Commands.ReservationCommands.ReservationCreateCommands;

public class ReservationCreateCommandHandler : IRequestHandler<ReservationCreateCommandRequest, ReservationCreateCommandResponse>
{
	private readonly IReservationRepository _reservationRepository;
	private readonly IRoomRepository _roomRepository;
	private readonly UserManager<AppUser> _userManager;
	private readonly IMapper _mapper;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IEmailService _emailService;

	public ReservationCreateCommandHandler(IReservationRepository reservationRepository,
		IRoomRepository roomRepository,
		UserManager<AppUser> userManager,
		IMapper mapper,
		IHttpContextAccessor httpContextAccessor,
		IEmailService emailService)
	{
		_reservationRepository = reservationRepository;
		_roomRepository = roomRepository;
		_userManager = userManager;
		_mapper = mapper;
		_httpContextAccessor = httpContextAccessor;
		_emailService = emailService;
	}

	public async Task<ReservationCreateCommandResponse> Handle(ReservationCreateCommandRequest request, CancellationToken cancellationToken)
	{
		var room = await _roomRepository.Table.Include(x=>x.Hotel).ThenInclude(x=>x.Rooms).ThenInclude(x=>x.Discounts).AsSplitQuery().FirstOrDefaultAsync(x=>x.Id==request.RoomId);
		if (room is null || room.IsDeactive)
		{
			throw new ArgumentException("Room not found or inactive.");
		}
		AppUser user = await _userManager.FindByIdAsync(request.UserId);
		if (user is null)
			throw new NotFoundException("User not found");
		var existingReservation = await _reservationRepository.Table
			.Where(r => r.RoomId == request.RoomId && r.IsCancelled==false && r.IsDeactive==false &&
						((r.StartTime <= request.EndTime && r.EndTime >= request.StartTime) ||
						 (r.StartTime >= request.StartTime && r.EndTime <= request.EndTime)))
			.FirstOrDefaultAsync();

		if (existingReservation is not null)
			throw new ArgumentException("Room already reserved for the specified time.");
		Reservation reservation=_mapper.Map<Reservation>(request);
		int nights=(request.EndTime - request.StartTime).Days;
		object[] price = await UpdateRoomDiscountedPrice(room);
		decimal roomDiscountedPricePerNight = (decimal)price[0];
		reservation.TotalPrice = roomDiscountedPricePerNight*((int)(reservation.EndTime-reservation.StartTime).TotalDays)+room.ServiceFee;
		reservation.DiscountPercent = (int)price[1];
		reservation.IsCancelled = false;
		reservation.IsPaid = request.IsPaid;
		reservation.AppUser = user;
		reservation.AppUserId = user.Id;
		room.IsReserved = true;

		await _reservationRepository.CreateAsync(reservation);
		string subject = "Reservation";
		if (request.IsPaid)
		{
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email", "paidreservation.html");
			string html = File.ReadAllText(filePath);
			html = html.Replace("{{hotel_name}}", room.Hotel.Name);
			html = html.Replace("{{room_type}}", room.RoomName);
			html = html.Replace("{{payment_details}}", (room.DiscountedPricePerNight * nights + room.ServiceFee).ToString());
			html = html.Replace("{{checkin_date}}", request.StartTime.ToString("yyyy-MM-dd"));
			html = html.Replace("{{checkout_date}}", request.EndTime.ToString("yyyy-MM-dd"));
			await _emailService.SendEmail(user.Email, subject, html);
		}
		if (!request.IsPaid)
		{
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email", "nonpaidreservation.html");
			string html = File.ReadAllText(filePath);
			html = html.Replace("{{hotel_name}}", room.Hotel.Name);
			html = html.Replace("{{room_type}}", room.RoomName);
			html = html.Replace("{{payment_details}}", (room.DiscountedPricePerNight * nights + room.ServiceFee).ToString());
			html = html.Replace("{{checkin_date}}", request.StartTime.ToString("yyyy-MM-dd"));
			html = html.Replace("{{checkout_date}}", request.EndTime.ToString("yyyy-MM-dd"));
			await _emailService.SendEmail(user.Email, subject, html);
		}
		await _reservationRepository.CommitAsync();
		return new ReservationCreateCommandResponse()
		{
			TotalPrice=reservation.TotalPrice
		};
	}
	private async Task<object[]> UpdateRoomDiscountedPrice(Room room)
	{
		decimal discountedPrice = room.PricePerNight;
		int discPercent = 0;

		foreach (var discount in room.Discounts)
		{
			if (!discount.IsDeactive &&
				discount.StartTime <= DateTime.Now &&
				discount.EndTime >= DateTime.Now)
			{
				decimal discountPercentage = discount.Percent / 100m;
				decimal discountedAmount = room.PricePerNight * discountPercentage;
				int roundedDiscountedAmount = (int)Math.Round(discountedAmount);
				discountedPrice -= roundedDiscountedAmount;
				discPercent = discount.Percent;
			}
		}

		return [discountedPrice,discPercent];
	}
}
