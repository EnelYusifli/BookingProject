using BookingProject.Application.Features.DTOs;
using BookingProject.MVC.ViewModels.RoomViewModels;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.HotelViewModels;

public class HotelCreateViewModel
{
	[Required(ErrorMessage ="Type Id is required")]
	public int TypeId { get; set; }

	public string? UserId { get; set; }

	[Required(ErrorMessage = "Country Id is required")]
	public int CountryId { get; set; }

	[Required]
	[StringLength(50, MinimumLength = 2)]
	public string Name { get; set; }

	[Required]
	[StringLength(1000, MinimumLength = 20)]
	public string Desc { get; set; }

	[Required]
	[StringLength(500)]
	public string Address { get; set; }

	[Required]
	[StringLength(50)]
	public string City { get; set; }

	[Required]
	public List<RoomCreateViewModel> RoomCreateDtos { get; set; }

	[MaxStringLengthInList(200, ErrorMessage = "Each item in HotelAdvantageNames must not exceed 200 characters.")]
	public List<string>? HotelAdvantageNames { get; set; }

	[Required]
	[MinLength(4, ErrorMessage = "At least 4 images are required.")]
	public List<IFormFile> ImageFiles { get; set; }
	[Required(ErrorMessage ="At least 1 language is required")]
	public List<int> StaffLanguageIds { get; set; }
	[Required(ErrorMessage = "At least 1 service is required")]

	public List<int> ServiceIds { get; set; }
	[Required(ErrorMessage = "At least 1 payment method is required")]

	public List<int> PaymentMethodIds { get; set; }
	[Required(ErrorMessage = "At least 1 activity is required")]

	public List<int> ActivityIds { get; set; }
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class MaxStringLengthInListAttribute : ValidationAttribute
	{
		private readonly int _maxLength;

		public MaxStringLengthInListAttribute(int maxLength)
		{
			_maxLength = maxLength;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is List<string> list)
			{
				foreach (var item in list)
				{
					if (item != null && item.Length > _maxLength)
					{
						return new ValidationResult($"Each item in {validationContext.DisplayName} must not exceed {_maxLength} characters.");
					}
				}
			}

			return ValidationResult.Success;
		}
	}
}
