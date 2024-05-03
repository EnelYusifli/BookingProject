namespace BookingProject.Domain.Entities.Commons;

public interface IBaseException
{
    public string? Message { get; }
    public int StatusCode { get; }
}
