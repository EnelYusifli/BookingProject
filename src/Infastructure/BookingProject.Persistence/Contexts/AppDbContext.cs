﻿using BookingProject.Domain.Entities;
using BookingProject.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Persistence.Contexts;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppUserConfiguration).Assembly);
        modelBuilder.Entity<Hotel>()
        .Property(h => h.StarPoint)
        .HasColumnType("decimal(18,1)");
        modelBuilder.Entity<Room>()
        .Property(h => h.PricePerNight)
        .HasColumnType("decimal(18,1)");
        modelBuilder.Entity<Room>()
        .Property(h => h.Area)
        .HasColumnType("decimal(18,1)");
        modelBuilder.Entity<Room>()
        .Property(h => h.ServiceFee)
        .HasColumnType("decimal(18,1)");
    }
    public DbSet<Hotel> Hotels { get; set; }    
    public DbSet<Activity> Activities { get; set; }    
    public DbSet<Service> Services { get; set; }    
    public DbSet<PaymentMethod> PaymentMethods { get; set; }    
    public DbSet<HotelStaffLanguage> HotelStaffLanguages { get; set; }    
    public DbSet<StaffLanguage> StaffLanguages { get; set; }    
    public DbSet<HotelPaymentMethod> HotelPaymentMethods { get; set; }    
    public DbSet<HotelActivity> HotelActivities { get; set; }    
    public DbSet<HotelService> HotelServices { get; set; }    
    public DbSet<HotelAdvantage> HotelAdvantages { get; set; }
    public DbSet<BookingProject.Domain.Entities.Type> Types { get; set; }
    public DbSet<CustomerReview> CustomerReviews { get; set; }
    public DbSet<UserWishlistHotel> UserWishlistHotels { get; set; }    
    public DbSet<Room> Rooms { get; set; }    
}