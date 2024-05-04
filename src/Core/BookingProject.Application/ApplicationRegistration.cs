using BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;
using BookingProject.Application.Mappings;
using BookingProject.Application.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
namespace BookingProject.Application;

public static class ApplicationRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
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
