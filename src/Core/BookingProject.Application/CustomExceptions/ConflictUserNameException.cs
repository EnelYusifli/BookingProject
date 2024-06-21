using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Application.CustomExceptions;

public class ConflictUserNameException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public ConflictUserNameException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 409;
    }
}