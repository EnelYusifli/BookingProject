using BookingProject.Application.Features.DTOs;
using BookingProject.Domain.Entities.Commons;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace BookingProject.API.Middlewares;

public static class ExceptionHandlerMiddeleware
{
    public static IApplicationBuilder UseCustomExceptionhandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                int StatusCode = (int)HttpStatusCode.InternalServerError;
                string message = "Internal Server Error";

                if (contextFeature is not null)
                {
                    if (contextFeature.Error is IBaseException)
                    {
                        var exception = (IBaseException)contextFeature.Error;
                        StatusCode = exception.StatusCode;
                        message = exception.Message;
                    }
                    else
                    {

                        var exception = contextFeature.Error;
                        message = exception.Message;
                    }
                }
                context.Response.StatusCode = StatusCode;
                await context.Response.WriteAsJsonAsync(new ExceptionResponseDto(StatusCode, message));
            });
        });
        return app;
    }
}
