using BookingProject.Application.Features.Commands.RoomCommands.RoomUpdateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.RoomValidators;

public class RoomUpdateCommandValidator:AbstractValidator<RoomUpdateCommandRequest>
{
    public RoomUpdateCommandValidator()
    {
		RuleFor(x => x.RoomName).NotEmpty().NotNull().MaximumLength(50);
		RuleFor(x => x.AdultCount).NotNull().GreaterThanOrEqualTo(1);
		RuleFor(x => x.Id).NotNull().GreaterThanOrEqualTo(1);
		RuleFor(x => x.ChildCount).NotNull().GreaterThanOrEqualTo(0);
		RuleFor(x => x.ServiceFee).NotNull().GreaterThanOrEqualTo(1);
		RuleFor(x => x.PricePerNight).NotNull().GreaterThanOrEqualTo(1);
		RuleFor(x => x.Area).NotNull().GreaterThanOrEqualTo(1);
		RuleFor(x => x.CancelAfterDay).NotNull().GreaterThanOrEqualTo(0);
		RuleFor(x => x.IsDeactive).NotNull();
		RuleFor(x => x.IsCancellable).NotNull();
	}
}
