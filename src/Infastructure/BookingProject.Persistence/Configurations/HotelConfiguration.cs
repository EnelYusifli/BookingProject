using BookingProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingProject.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.Property(x=>x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x=>x.Desc).IsRequired().HasMaxLength(1000);
        builder.Property(x=>x.Address).IsRequired().HasMaxLength(500);
        builder.Property(x=>x.Country).IsRequired().HasMaxLength(50);
        builder.Property(x=>x.City).IsRequired().HasMaxLength(50);
        //builder.HasMany(x => x.HotelAdvantages)
        //   .WithOne(x => x.Hotel)
        //   .HasForeignKey(x => x.HotelId)
        //   .OnDelete(DeleteBehavior.Cascade);
        //builder.HasMany(x => x.HotelActivities)
        //   .WithOne(x => x.Hotel)
        //   .HasForeignKey(x => x.HotelId)
        //   .OnDelete(DeleteBehavior.Cascade);
    }
}
