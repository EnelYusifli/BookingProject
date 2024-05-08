using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.ReviewCommands.ReviewDeleteCommands;
using BookingProject.Application.Helpers.Extensions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewDeleteCommands;

public class ReviewDeleteCommandHandler : IRequestHandler<ReviewDeleteCommandRequest, ReviewDeleteCommandResponse>
{
	private readonly IReviewRepository _repository;
	private readonly IConfiguration _configuration;
	private readonly IHotelRepository _hotelRepository;

	public ReviewDeleteCommandHandler(IReviewRepository repository, IConfiguration configuration,IHotelRepository hotelRepository)
	{
		_repository = repository;
		_configuration = configuration;
		_hotelRepository = hotelRepository;
	}
	public async Task<ReviewDeleteCommandResponse> Handle(ReviewDeleteCommandRequest request, CancellationToken cancellationToken)
	{
		CustomerReview review = await _repository.Table.Include(x => x.ReviewImages).Include(x=>x.Hotel).FirstOrDefaultAsync(x => x.Id == request.Id);
		if (review is null) throw new NotFoundException("Review not found");
		if(review.ReviewImages is not null)
		{
		    SaveFileExtension.Initialize(_configuration);
			foreach (var image in review.ReviewImages)
			{
				await SaveFileExtension.DeleteFileAsync(image.Url);
			}
		}
		review.Hotel.StarPoint =
			(await _hotelRepository.GetTotalStarPointsAsync(review.HotelId) - review.StarPoint) / (await _hotelRepository.GetNumberOfReviewsAsync(review.HotelId) - 1);
		_repository.Delete(review);
		await _repository.CommitAsync();
		return new ReviewDeleteCommandResponse();
	}
}
