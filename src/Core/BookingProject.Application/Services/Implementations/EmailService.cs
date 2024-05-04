using BookingProject.Application.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BookingProject.Application.Services.Implementations;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendEmail(string to, string subject, string html)
    {
        var email = new MimeMessage();
        email.Subject = subject;
        email.To.Add(MailboxAddress.Parse(to));
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = html };

        email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:from"]));

        using var client = new SmtpClient();
        await client.ConnectAsync(_configuration["EmailSettings:host"], 587, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_configuration["EmailSettings:from"], _configuration["EmailSettings:password"]);

        await client.SendAsync(email);

        await client.DisconnectAsync(true);
    }


}
