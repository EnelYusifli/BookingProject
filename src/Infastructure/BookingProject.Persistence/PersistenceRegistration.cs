using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;
using BookingProject.Persistence.Repositories;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using BookingProject.Application.Services.Implementations;
using BookingProject.Application.Services.Interfaces;


namespace BookingProject.Persistence;

public static class PersistenceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
		services.AddHttpContextAccessor();
		services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IReviewImageRepository, ReviewImageRepository>();
        services.AddScoped<IRoomImageRepository, RoomImageRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IHotelImageRepository, HotelImageRepository>();
        services.AddScoped<IHotelActivityRepository, HotelActivityRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<IHotelPaymentMethodRepository, HotelPaymentMethodRepository>();
        services.AddScoped<IStaffLanguageRepository, StaffLanguageRepository>();
        services.AddScoped<IHotelStaffLanguageRepository, HotelStaffLanguageRepository>();
        services.AddScoped<IAdvantageRepository, AdvantageRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IHotelServiceRepository, HotelServiceRepository>();
        services.AddAuthorization();
        //     services.AddIdentityApiEndpoints<AppUser>()
        //.AddRoles<IdentityRole>()
        //.AddEntityFrameworkStores<AppDbContext>();
        services.AddIdentity<AppUser, IdentityRole>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequiredLength = 8;
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireLowercase = true;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            opt.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
		services.AddScoped<UserManager<AppUser>>();
		services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("default")));
		services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddCookie(x =>
        {
            x.Cookie.Name = "token";
			x.Cookie.Name = "refreshToken";
		}).AddGoogle(googleOptions =>
		{
			googleOptions.ClientId = configuration["Authentication:GoogleClientId"];
			googleOptions.ClientSecret = configuration["Authentication:GoogleClientSecret"];
		})
            .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = configuration.GetSection("JWT:audience").Value,
                ValidIssuer = configuration.GetSection("JWT:issuer").Value,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:securityKey").Value)),

                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["token"];
                    return Task.CompletedTask;
                }
            };
        });
		//services.AddCors(options =>
		//{
		//	options.AddPolicy("AllowSpecificOrigin",
		//		builder =>
		//		{
		//			builder.WithOrigins("https://localhost:7183")
		//				.AllowAnyHeader()
		//				.AllowAnyMethod()
		//				.AllowCredentials();
		//		});
		//});

	}
}
