using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Application.CustomExceptions;

public class ServerErrorException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public ServerErrorException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 500;
    }
}