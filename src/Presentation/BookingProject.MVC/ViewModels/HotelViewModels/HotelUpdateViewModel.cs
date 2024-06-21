using BookingProject.Application.Features.DTOs;
using BookingProject.MVC.ViewModels.PropertyViewModels;
using BookingProject.MVC.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.HotelViewModels
{
	public class HotelUpdateViewModel
	{
		public int Id { get; set; }

		public string? UserId { get; set; }

		[Required(ErrorMessage = "TypeId is required.")]
		public int TypeId { get; set; }

		[Required(ErrorMessage = "CountryId is required.")]
		public int CountryId { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Desc is required.")]
		[StringLength(1000, MinimumLength = 20, ErrorMessage = "Desc must be between 20 and 1000 characters.")]
		public string Desc { get; set; }

		[Required(ErrorMessage = "Address is required.")]
		[StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
		public string Address { get; set; }

		[Required(ErrorMessage = "City is required.")]
		[StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
		public string City { get; set; }
		//[EnsureMinimumImages(ErrorMessage = "At least 4 images must remain after deletions.")]

		public List<IFormFile>? NewImageFiles { get; set; }

		public List<ImageDto>? Images { get; set; }

		public List<AdvantageDto>? Advantages { get; set; }

		[Required(ErrorMessage = "At least 1 language is required")]
		public List<int> StaffLanguageIds { get; set; }
		[Required(ErrorMessage = "At least 1 service is required")]

		public List<int> ServiceIds { get; set; }
		[Required(ErrorMessage = "At least 1 payment method is required")]

		public List<int> PaymentMethodIds { get; set; }
		[Required(ErrorMessage = "At least 1 activity is required")]

		public List<int> ActivityIds { get; set; }
		[MaxStringLengthInList(200, ErrorMessage = "Each item in HotelAdvantageNames must not exceed 200 characters.")]

		public List<string>? HotelAdvantageNames { get; set; }
		[ExcludeFromValidation]
		public List<int>? DeletedAdvantageIds { get; set; }
		[ExcludeFromValidation]
		public List<int>? DeletedImageFileIds { get; set; }

		public List<RoomGetViewModel>? Rooms { get; set; }

		public PropertyViewModel? Property { get; set; }
	}
	public class ExcludeFromValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			return ValidationResult.Success;
		}
	}
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
	public class EnsureMinimumImagesAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var viewModel = validationContext.ObjectInstance as HotelUpdateViewModel;

			if (viewModel != null)
			{
				int currentImageCount = viewModel.Images?.Count ?? 0;
				int deletedImageCount = (value as List<int>)?.Count ?? 0;
				int newImageCount = viewModel.NewImageFiles?.Count ?? 0;

				// Calculate total images after considering deletions and additions
				int totalImages = currentImageCount - deletedImageCount + newImageCount;

				if (totalImages < 4)
				{
					return new ValidationResult("At least 4 images must remain after deletions.");
				}
			}

			return ValidationResult.Success;
		}
	}

}
