using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Application.CustomExceptions;

public class NotFoundException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public NotFoundException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 404;
    }
}
