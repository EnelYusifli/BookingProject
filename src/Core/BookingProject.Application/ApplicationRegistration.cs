using BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;
using BookingProject.Application.Mappings;
using BookingProject.Application.Services.Implementations;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Application.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
namespace BookingProject.Application;

public static class ApplicationRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenService,TokenService>();
        services.AddScoped<IRoomService, RoomService>();

        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssemblies(typeof(AuthLoginCommandRequest).Assembly);
        });
        services.AddValidatorsFromAssemblyContaining<AuthRegisterCommandRequestValidator>();
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddAutoMapper(typeof(AuthMappingProfile).Assembly);
    }
}
