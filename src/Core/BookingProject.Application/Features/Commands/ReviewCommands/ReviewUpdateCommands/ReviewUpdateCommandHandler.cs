using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Helpers.Extensions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewUpdateCommands
{
	public class ReviewUpdateCommandHandler : IRequestHandler<ReviewUpdateCommandRequest, ReviewUpdateCommandResponse>
	{
		private readonly IReviewRepository _repository;
		private readonly IMapper _mapper;
		private readonly IReviewImageRepository _reviewImageRepository;

		public ReviewUpdateCommandHandler(IReviewRepository repository, IMapper mapper, IReviewImageRepository reviewImageRepository)
		{
			_repository = repository;
			_mapper = mapper;
			_reviewImageRepository = reviewImageRepository;
		}

		public async Task<ReviewUpdateCommandResponse> Handle(ReviewUpdateCommandRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
				throw new NotFoundException("Request not found");

			CustomerReview review = await _repository.GetByIdAsync(request.Id);
			if (review is null)
				throw new NotFoundException($"Review with ID {request.Id} not found");


			if (request.ReviewImages is not null)
			{
				foreach (var image in request.ReviewImages)
				{
					if (image is null)
						throw new NotFoundException($"Image not found");

					string url = await SaveFileExtension.SaveFile(image, "reviews");
					ReviewImage reviewImage = new ReviewImage
					{
						Review = review,
						Url = url,
						IsDeactive = false
					};
					await _reviewImageRepository.CreateAsync(reviewImage);
				}
			}

			if (request.DeletedImageFileIds is not null)
			{
				foreach (var id in request.DeletedImageFileIds)
				{
					ReviewImage image = await _reviewImageRepository.GetByIdAsync(id);
					if (image is null || image.ReviewId != review.Id)
						throw new NotFoundException($"Image with ID {id} not found in review");

					_reviewImageRepository.Delete(image);
					await SaveFileExtension.DeleteFileAsync(image.Url);
				}
			}

			review.ModifiedDate = DateTime.Now;
			_mapper.Map(request, review);

			await _repository.CommitAsync();

			return new ReviewUpdateCommandResponse();
		}
	}
}
