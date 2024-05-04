using BookingProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingProject.Persistence.Configurations;

public class HotelAdvantageConfiguration : IEntityTypeConfiguration<HotelAdvantage>
{
    public void Configure(EntityTypeBuilder<HotelAdvantage> builder)
    {
        builder.Property(x=>x.AdvantageName).IsRequired().HasMaxLength(200);
    }
}
