using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.RoomViewModels;

public class DiscountCreateViewModel
{
	public int RoomId { get; set; }
	[Range(1, 100, ErrorMessage = "Percent must be between 1 and 100.")]
	public int Percent { get; set; }
	[Required(ErrorMessage = "StartTime is required.")]
	[FutureDate(ErrorMessage = "StartTime must be in the future.")]
	public DateTime StartTime { get; set; }

	[Required(ErrorMessage = "EndTime is required.")]
	[GreaterThan(nameof(StartTime), ErrorMessage = "EndTime must be greater than StartTime.")]
	public DateTime EndTime { get; set; }
	public class FutureDateAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			DateTime dateTime = (DateTime)value;
			return dateTime > DateTime.Now;
		}
	}
	public class GreaterThanAttribute : ValidationAttribute
	{
		private readonly string _comparisonProperty;

		public GreaterThanAttribute(string comparisonProperty)
		{
			_comparisonProperty = comparisonProperty;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var propertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);
			if (propertyInfo == null)
			{
				return new ValidationResult($"Property {_comparisonProperty} not found.");
			}

			var comparisonValue = (DateTime)propertyInfo.GetValue(validationContext.ObjectInstance);
			var currentValue = (DateTime)value;

			if (currentValue <= comparisonValue)
			{
				return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be greater than {_comparisonProperty}.");
			}

			return ValidationResult.Success;
		}
	}

}
