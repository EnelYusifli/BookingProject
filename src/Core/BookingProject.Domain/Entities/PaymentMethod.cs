using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class PaymentMethod : BaseEntity, IBaseAuditable
{
    public DateTime CreatedDate { get; } = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public string PaymentMethodName { get; set; }
    public List<HotelPaymentMethod> HotelPaymentMethods { get; set; }
}
