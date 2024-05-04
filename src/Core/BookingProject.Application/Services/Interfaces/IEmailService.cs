namespace BookingProject.Application.Services.Interfaces;

public interface IEmailService
{
    Task SendEmail(string to, string subject, string html);
}
