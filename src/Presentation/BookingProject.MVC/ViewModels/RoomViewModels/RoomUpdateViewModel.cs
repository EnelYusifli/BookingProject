﻿using BookingProject.Application.Features.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.RoomViewModels
{
	public class RoomUpdateViewModel
	{
		[Required(ErrorMessage = "RoomName is required.")]
		[StringLength(50, ErrorMessage = "RoomName cannot exceed 50 characters.")]
		public string RoomName { get; set; }

		public string? UserId { get; set; }

		public List<ImageDto>? Images { get; set; }
		public int? ImageCount { get; set; }

		[Required(ErrorMessage = "Id is required.")]
		public int Id { get; set; }

		[Required(ErrorMessage = "IsDeactive is required.")]
		public bool IsDeactive { get; set; }

		[Required(ErrorMessage = "AdultCount is required.")]
		[Range(1, int.MaxValue, ErrorMessage = "AdultCount must be greater than or equal to 1.")]
		public int AdultCount { get; set; }

		[Required(ErrorMessage = "ChildCount is required.")]
		[Range(0, int.MaxValue, ErrorMessage = "ChildCount must be greater than or equal to 0.")]
		public int ChildCount { get; set; }

		[Required(ErrorMessage = "IsDepositNeeded is required.")]
		public bool IsDepositNeeded { get; set; }

		[Required(ErrorMessage = "ServiceFee is required.")]
		[Range(1, double.MaxValue, ErrorMessage = "ServiceFee must be greater than or equal to 1.")]
		public decimal ServiceFee { get; set; }

		[Required(ErrorMessage = "PricePerNight is required.")]
		[Range(1, double.MaxValue, ErrorMessage = "PricePerNight must be greater than or equal to 1.")]
		public decimal PricePerNight { get; set; }

		[Required(ErrorMessage = "Area is required.")]
		[Range(1, double.MaxValue, ErrorMessage = "Area must be greater than or equal to 1.")]
		public decimal Area { get; set; }

		[Required(ErrorMessage = "IsCancellable is required.")]
		public bool IsCancellable { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "CancelAfterDay must be greater than or equal to 0.")]
		public int? CancelAfterDay { get; set; }

		//[EnsureMinimumImages(ErrorMessage = "At least 2 images must remain after deletions.")]
		public List<IFormFile>? ImageFiles { get; set; }
		public List<int>? DeletedImageFileIds { get; set; }
	}
	public class EnsureMinimumImagesAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var viewModel = validationContext.ObjectInstance as RoomUpdateViewModel;

			if (viewModel != null)
			{
				int currentImageCount = viewModel.ImageCount ?? 0;
				int deletedImageCount = viewModel.DeletedImageFileIds?.Count ?? 0;
				int newImageCount = viewModel.ImageFiles?.Count ?? 0;

				// Calculate total images after considering deletions and additions
				int totalImages = currentImageCount - deletedImageCount + newImageCount;

				// Check if at least 2 images will remain
				if (totalImages < 2)
				{
					return new ValidationResult("At least 2 images must remain after deletions.");
				}
			}

			return ValidationResult.Success;
		}
	}
}
