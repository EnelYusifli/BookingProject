using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class HotelPaymentMethod : BaseEntity, IBaseAuditable
{
    public int HotelId { get; set; }
    public int PaymentMethodId { get; set; }
    public Hotel Hotel { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

       public DateTime CreatedDate { get; set;} = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
}
