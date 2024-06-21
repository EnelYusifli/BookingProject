using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Application.CustomExceptions;

public class ConflictEmailException : Exception, IBaseException
{
	public int StatusCode { get; }
	public string? Message { get; }
	public ConflictEmailException(string? message = null) : base(message)
	{
		Message = message;
		StatusCode = 410;
	}
}