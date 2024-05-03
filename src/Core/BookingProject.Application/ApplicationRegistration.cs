using Microsoft.Extensions.DependencyInjection;

namespace BookingProject.Application;

public static class ApplicationRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        //services.AddAutoMapper(typeof(BlogMappingProfile).Assembly);
        //services.AddMediatR(opt =>
        //{
        //    opt.RegisterServicesFromAssemblies(typeof(BlogGetAllQueryRequest).Assembly);
        //});
        //services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BlogCreateCommandRequestValidator>());

    }
}
