using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;

namespace BookingProject.Application.Features.Commands.ReviewCommands.ReviewReportCommands;

public class ReviewReportCommandHandler : IRequestHandler<ReviewReportCommandRequest, ReviewReportCommandResponse>
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewReportCommandHandler(IReviewRepository reviewRepository)
    {
       _reviewRepository = reviewRepository;
    }
    public async Task<ReviewReportCommandResponse> Handle(ReviewReportCommandRequest request, CancellationToken cancellationToken)
    {
        if (request is null) throw new NotFoundException("Request not found");
        if (request.Id < 0) throw new BadRequestException("Id must be positive");
        CustomerReview review= await _reviewRepository.GetByIdAsync(request.Id);
        if (review is null || review.IsDeactive == true) throw new NotFoundException("Review not found");
        //review.IsDeactive = true;
        if(review.IsReported==false)
        review.IsReported = true;
        if (review.IsReported == true)
            review.IsReported = false;
        await _reviewRepository.CommitAsync();

        return new ReviewReportCommandResponse();
    }

}
