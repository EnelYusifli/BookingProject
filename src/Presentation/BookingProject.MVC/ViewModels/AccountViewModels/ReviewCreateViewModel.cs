﻿using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class ReviewCreateViewModel
{
	public int HotelId { get; set; }
	public string UserId { get; set; }
	[Range(0, 5, ErrorMessage = "StarPoint must be between 0 and 5.")]
	[Required]
	public int StarPoint { get; set; }
	[MaxLength(200)]
	[Required]
	public string ReviewMessage { get; set; }
	public List<IFormFile>? ReviewImageFiles { get; set; }
}
