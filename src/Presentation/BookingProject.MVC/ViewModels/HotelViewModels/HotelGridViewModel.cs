namespace BookingProject.MVC.ViewModels.HotelViewModels;

public class HotelGridViewModel
{
	public decimal? minPrice{get;set;}
	public decimal? maxPrice{get;set;}
	public decimal? starPoint{get;set;}
	public string? typeName{get;set;}
	public bool? mostPopular{get;set;}
	public bool? mostRated{get;set;}
	public string? searchStr{get;set;}
	public string? dateRange { get;set;}
	public int? adultCount{get;set;}
	public int? roomCount{get;set;}
	public int? childCount{get;set;}
	public string? countryName{get;set;}
	public int page { get; set; } = 1;
	public int itemPerPage { get; set; } = 5;
}
