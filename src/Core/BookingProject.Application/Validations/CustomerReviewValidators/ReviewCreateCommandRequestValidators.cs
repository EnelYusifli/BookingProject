using BookingProject.Application.Features.Commands.ReviewCommands.ReviewCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.CustomerReviewValidators;

public class ReviewCreateCommandRequestValidators:AbstractValidator<ReviewCreateCommandRequest>
{
    public ReviewCreateCommandRequestValidators()
    {
        RuleFor(x=>x.HotelId).NotNull().NotEmpty().GreaterThanOrEqualTo(1);
        RuleFor(x=>x.StarPoint).NotNull().NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
        RuleFor(x=>x.UserId).NotNull().NotEmpty();
        RuleFor(x=>x.ReviewMessage).NotNull().NotEmpty().MaximumLength(200);
		RuleFor(x => x.ReviewImages)
	            .Must(images => images == null || images.Count <= 5)
	            .WithMessage("At most 5 images can be uploaded.");
	}
}
