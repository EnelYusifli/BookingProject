using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Helpers.Extensions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
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

	public ReviewCreateCommandHandler(IReviewRepository repository
		,IMapper mapper
		,IReviewImageRepository reviewImageRepository
		,IHotelRepository hotelRepository
		,UserManager<AppUser> userManager
		,IConfiguration configuration)
    {
		_repository = repository;
		_mapper = mapper;
		_reviewImageRepository = reviewImageRepository;
		_hotelRepository = hotelRepository;
		_userManager = userManager;
		_configuration = configuration;
	}
    public async Task<ReviewCreateCommandResponse> Handle(ReviewCreateCommandRequest request, CancellationToken cancellationToken)
	{
		AppUser user = await _userManager.FindByIdAsync(request.UserId);
		if (user is null)
			throw new NotFoundException("User not found");
		Hotel hotel= await _hotelRepository.GetByIdAsync(request.HotelId);
		if(hotel is null)
			throw new NotFoundException("Hotel not found");
		CustomerReview review = _mapper.Map<CustomerReview>(request);
		review.IsDeactive = false;
		SaveFileExtension.Initialize(_configuration);
		if(request.ReviewImages is not null)
		{
			foreach (var image in request.ReviewImages)
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
				await _reviewImageRepository.CreateAsync(img);
			}
		}
		
		hotel.StarPoint = 
			(await _hotelRepository.GetTotalStarPointsAsync(request.HotelId)+request.StarPoint)/(await _hotelRepository.GetNumberOfReviewsAsync(request.HotelId)+1);
		await _repository.CreateAsync(review);
		await _repository.CommitAsync();
		return new ReviewCreateCommandResponse();
	}
}
