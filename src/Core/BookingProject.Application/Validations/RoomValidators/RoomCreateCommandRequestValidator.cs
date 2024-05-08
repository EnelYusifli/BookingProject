using BookingProject.Application.Features.Commands.RoomCommands.RoomCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.RoomValidators;

public class RoomCreateCommandRequestValidator:AbstractValidator<RoomCreateCommandRequest>
{
    public RoomCreateCommandRequestValidator()
    {
        RuleFor(x=>x.RoomName).NotEmpty().NotNull().MaximumLength(50);
        RuleFor(x=>x.HotelId).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x=>x.AdultCount).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x=>x.ChildCount).NotNull().GreaterThanOrEqualTo(0);
        RuleFor(x=>x.ServiceFee).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x=>x.PricePerNight).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x=>x.Area).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x=>x.CancelAfterDay).NotNull().GreaterThanOrEqualTo(0);
        RuleFor(x=>x.IsDeactive).NotNull();
        RuleFor(x=>x.IsCancellable).NotNull();
        RuleFor(x => x.ImageFiles).NotNull()
           .Must(images => images != null && images.Count >= 2)
               .WithMessage("At least 2 images are required.");
    }
}
