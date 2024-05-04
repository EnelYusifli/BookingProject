using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Application.CustomExceptions;

public class ConflictException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public ConflictException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 409;
    }
}