using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class OfferUsers:BaseEntity
{
	public AppUser AppUser { get; set; }
	public Offer Offer { get; set; }
	public string AppUserId { get; set; }
	public int OfferId { get; set; }
}
