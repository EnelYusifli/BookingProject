using BookingProject.Domain.Entities;
using BookingProject.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Persistence.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		modelBuilder.Entity<Hotel>()
		.HasOne(h => h.AppUser)
		.WithMany(u => u.Hotels)
		.HasForeignKey(h => h.AppUserId)
		.OnDelete(DeleteBehavior.NoAction);
		base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppUserConfiguration).Assembly);
        modelBuilder.Entity<Hotel>()
        .Property(h => h.StarPoint)
        .HasColumnType("decimal(18,1)");
        modelBuilder.Entity<Room>()
        .Property(h => h.PricePerNight)
        .HasColumnType("decimal(18,1)");
		modelBuilder.Entity<Room>()
	   .Property(h => h.DiscountedPricePerNight)
	   .HasColumnType("decimal(18,1)");
		modelBuilder.Entity<Room>()
        .Property(h => h.Area)
        .HasColumnType("decimal(18,1)");
        modelBuilder.Entity<Room>()
        .Property(h => h.ServiceFee)
        .HasColumnType("decimal(18,1)");
		modelBuilder.Entity<About>().HasData(
		   new About
		   {
			   Id = 1,
			   StoryTitle = "Founded in 2006, passage its ten led hearted removal cordial. Preference any astonished unreserved Mrs. Prosperous understood Middletons in conviction an uncommonly do. Supposing so be resolving breakfast am or perfectly. It drew a hill from me. Valley by oh twenty direct me so.",
			   Story = "Water timed folly right aware if oh truth. Imprudence attachment him his for sympathize."
		   }
	   );
		//modelBuilder.Entity<TermsOfService>().HasData(
		//   new TermsOfService
		//   {
		//	   Id = 2,
		//	   Title = "Role of Booking and Limitation of Liability of Booking",
  //             Text="Text"
		//   }
	 //  );
	}
    public DbSet<Hotel> Hotels { get; set; }    
    public DbSet<FAQ> FAQs { get; set; }    
    public DbSet<Message> Messages { get; set; }    
    public DbSet<About> About { get; set; }    
    public DbSet<TermsOfService> TermsOfService { get; set; }    
    public DbSet<Country> Countries { get; set; }    
    public DbSet<Discount> Discounts { get; set; }    
    public DbSet<Offer> Offers { get; set; }    
    public DbSet<UserCard> Cards { get; set; }    
    public DbSet<Reservation> Reservations { get; set; }    
    public DbSet<Activity> Activities { get; set; }    
    public DbSet<Service> Services { get; set; }    
    public DbSet<PaymentMethod> PaymentMethods { get; set; }    
    public DbSet<HotelStaffLanguage> HotelStaffLanguages { get; set; }    
    public DbSet<StaffLanguage> StaffLanguages { get; set; }    
    public DbSet<HotelPaymentMethod> HotelPaymentMethods { get; set; }    
    public DbSet<HotelActivity> HotelActivities { get; set; }    
    public DbSet<HotelImage> HotelImages { get; set; }    
    public DbSet<RoomImage> RoomImages { get; set; }    
    public DbSet<ReviewImage> ReviewImages { get; set; }    
    public DbSet<HotelService> HotelServices { get; set; }    
    public DbSet<HotelAdvantage> HotelAdvantages { get; set; }
    public DbSet<BookingProject.Domain.Entities.Type> Types { get; set; }
    public DbSet<CustomerReview> CustomerReviews { get; set; }
    public DbSet<UserWishlistHotel> UserWishlistHotels { get; set; }    
    public DbSet<Room> Rooms { get; set; }    
}
