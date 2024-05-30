using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Helpers.Extensions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewCreateCommands;

public class ReviewCreateCommandHandler : IRequestHandler<ReviewCreateCommandRequest, ReviewCreateCommandResponse>
{
	private readonly IReviewRepository _repository;
	private readonly IMapper _mapper;
	private readonly IReviewImageRepository _reviewImageRepository;
	private readonly IHotelRepository _hotelRepository;
	private readonly UserManager<AppUser> _userManager;
	private readonly IConfiguration _configuration;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IReservationRepository _reservationRepository;

	public ReviewCreateCommandHandler(IReviewRepository repository
		,IMapper mapper
		,IReviewImageRepository reviewImageRepository
		,IHotelRepository hotelRepository
		,UserManager<AppUser> userManager
		,IConfiguration configuration
		,IHttpContextAccessor httpContextAccessor
		,IReservationRepository reservationRepository)
    {
		_repository = repository;
		_mapper = mapper;
		_reviewImageRepository = reviewImageRepository;
		_hotelRepository = hotelRepository;
		_userManager = userManager;
		_configuration = configuration;
		_httpContextAccessor = httpContextAccessor;
		_reservationRepository = reservationRepository;
	}
	public async Task<ReviewCreateCommandResponse> Handle(ReviewCreateCommandRequest request, CancellationToken cancellationToken)
	{
		AppUser user = new();
		if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
		{
			user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
			Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Console.WriteLine(user.NormalizedEmail);
		}
		else
		{
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Console.WriteLine("user.NormalizedEmail");
        }
		if (user is null)
			throw new NotFoundException("User not found");

		Hotel hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
		if (hotel is null)
			throw new NotFoundException("Hotel not found");

		bool hasPastReservation = await HasPastReservationAsync(user.Id, hotel.Id);

		if (!hasPastReservation)
			throw new InvalidOperationException("User must have a past reservation for the hotel to add a review.");

		CustomerReview review = _mapper.Map<CustomerReview>(request);
		review.IsDeactive = false;
		SaveFileExtension.Initialize(_configuration);
		if (request.ReviewImageFiles is not null)
		{
			foreach (var image in request.ReviewImageFiles)
			{
				if (image is null)
					throw new NotFoundException($"Image not found");
				string url = await SaveFileExtension.SaveFile(image, "reviews");
				ReviewImage img = new()
				{
					Review = review,
					Url = url,
					IsDeactive = false
				};
				review.ReviewImages.Add(img);
				await _reviewImageRepository.CreateAsync(img);
			}
		}

		review.User = user;
		review.UserId = user.Id;

		hotel.StarPoint =
			(await _hotelRepository.GetTotalStarPointsAsync(request.HotelId) + request.StarPoint) /
			(await _hotelRepository.GetNumberOfReviewsAsync(request.HotelId) + 1);

		await _repository.CreateAsync(review);
		await _repository.CommitAsync();

		return new ReviewCreateCommandResponse();
	}

	private async Task<bool> HasPastReservationAsync(string userId, int hotelId)
	{
		var pastReservations = await _reservationRepository.Table.Include(X=>X.Room)
			.Where(r => r.AppUserId == userId && r.Room.HotelId == hotelId && r.StartTime < DateTime.Now)
			.ToListAsync();

		return pastReservations.Any();
	}

}
