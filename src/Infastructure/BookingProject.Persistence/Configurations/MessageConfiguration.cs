using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookingProject.Domain.Entities;

namespace BookingProject.Persistence.Configurations;

public class MessageConfiguration: IEntityTypeConfiguration<Message>
{
	public void Configure(EntityTypeBuilder<Message> builder)
	{
		builder.Property(x => x.MessageText).IsRequired().HasMaxLength(1000);
		builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
		builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
	}
}
