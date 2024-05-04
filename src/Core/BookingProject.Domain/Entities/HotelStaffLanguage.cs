using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class HotelStaffLanguage : BaseEntity, IBaseAuditable
{
    public int HotelId { get; set; }
    public int StaffLanguageId { get; set; }
    public Hotel Hotel { get; set; }
    public StaffLanguage StaffLanguage { get; set; }

    public DateTime CreatedDate { get; } = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
}
