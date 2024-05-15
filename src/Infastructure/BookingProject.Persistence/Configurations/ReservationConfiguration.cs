using BookingProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingProject.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
	public void Configure(EntityTypeBuilder<Reservation> builder)
	{
		builder.Property(x => x.RoomId).IsRequired();
		builder.Property(x => x.AppUserId).IsRequired();
		builder.Property(x => x.StartTime).IsRequired();
		builder.Property(x => x.EndTime).IsRequired();
		builder.Property(x => x.IsPaid).IsRequired();
	}
}
