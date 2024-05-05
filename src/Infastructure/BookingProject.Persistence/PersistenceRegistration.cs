using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;
using BookingProject.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookingProject.Persistence;

public static class PersistenceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<IStaffLanguageRepository, StaffLanguageRepository>();
        services.AddScoped<IAdvantageRepository, AdvantageRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
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
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("default")));
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
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
        });

    }
}
