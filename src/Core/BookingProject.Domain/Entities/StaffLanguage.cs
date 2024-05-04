using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class StaffLanguage : BaseEntity, IBaseAuditable
{
    public DateTime CreatedDate { get; } = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public string StaffLanguageName { get; set; }
    public List<HotelStaffLanguage> HotelStaffLanguages { get; set; }
}
