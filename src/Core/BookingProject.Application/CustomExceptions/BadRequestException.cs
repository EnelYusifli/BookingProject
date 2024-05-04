using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Application.CustomExceptions;

public class BadRequestException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public BadRequestException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 400;
    }
}
