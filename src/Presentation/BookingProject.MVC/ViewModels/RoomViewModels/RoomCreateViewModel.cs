using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.RoomViewModels
{
	public class RoomCreateViewModel
	{
		[Required(ErrorMessage = "RoomName is required.")]
		[StringLength(50, ErrorMessage = "RoomName cannot exceed 50 characters.")]
		public string RoomName { get; set; }

		[Required(ErrorMessage = "HotelId is required.")]
		[Range(1, int.MaxValue, ErrorMessage = "HotelId must be greater than or equal to 1.")]
		public int? HotelId { get; set; }

		[Required(ErrorMessage = "AdultCount is required.")]
		[Range(1, int.MaxValue, ErrorMessage = "AdultCount must be greater than or equal to 1.")]
		public int AdultCount { get; set; }

		[Required(ErrorMessage = "ChildCount is required.")]
		[Range(0, int.MaxValue, ErrorMessage = "ChildCount must be greater than or equal to 0.")]
		public int ChildCount { get; set; }

		[Required(ErrorMessage = "ServiceFee is required.")]
		[Range(1, double.MaxValue, ErrorMessage = "ServiceFee must be greater than or equal to 1.")]
		public decimal ServiceFee { get; set; }

		[Required(ErrorMessage = "PricePerNight is required.")]
		[Range(1, double.MaxValue, ErrorMessage = "PricePerNight must be greater than or equal to 1.")]
		public decimal PricePerNight { get; set; }

		[Required(ErrorMessage = "Area is required.")]
		[Range(1, double.MaxValue, ErrorMessage = "Area must be greater than or equal to 1.")]
		public decimal Area { get; set; }

		[Required(ErrorMessage = "CancelAfterDay is required.")]
		[Range(0, int.MaxValue, ErrorMessage = "CancelAfterDay must be greater than or equal to 0.")]
		public int? CancelAfterDay { get; set; } = 0;

		[Required(ErrorMessage = "IsDepositNeeded is required.")]
		public bool IsDepositNeeded { get; set; }

		[Required(ErrorMessage = "IsCancellable is required.")]
		public bool IsCancellable { get; set; }

		public List<IFormFile> ImageFiles { get; set; }
	}
}
