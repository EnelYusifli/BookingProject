using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageCreateCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.HotelValidators;

public class HotelCreateCommandRequestValidator : AbstractValidator<HotelCreateCommandRequest>
{
    public HotelCreateCommandRequestValidator()
    {
        RuleFor(x=>x.Name).NotEmpty().NotNull().MaximumLength(50).MinimumLength(2);
        RuleFor(x=>x.Address).NotEmpty().NotNull().MaximumLength(500);
        RuleFor(x=>x.Country).NotEmpty().NotNull().MaximumLength(50);
        RuleFor(x=>x.City).NotEmpty().NotNull().MaximumLength(50);
        RuleFor(x=>x.TypeId).NotEmpty().NotNull();
        RuleFor(x => x.ImageFiles).NotNull()
           .Must(images => images != null && images.Count >= 4)
               .WithMessage("At least 4 images are required.");
        RuleFor(x=>x.Desc).NotEmpty().NotNull().MaximumLength(1000).MinimumLength(20);
        RuleFor(x=>x.AppUserId).NotEmpty().NotNull();
    }
}
