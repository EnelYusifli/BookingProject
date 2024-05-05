using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookingProject.Application.Features.Queries;

public class ForgotPasswordQueryHandler : IRequestHandler<ForgotPasswordQueryRequest, ForgotPasswordQueryResponse>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;

    public ForgotPasswordQueryHandler(IEmailService emailService,UserManager<AppUser> userManager)
    {
        _emailService = emailService;
        _userManager = userManager;
    }
    public async Task<ForgotPasswordQueryResponse> Handle(ForgotPasswordQueryRequest request, CancellationToken cancellationToken)
    {
        var user=await _userManager.FindByEmailAsync(request.Email);
        if (user is null) throw new NotFoundException("No account found with this email address.");
        string subject = "Forgot Password";
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email", "forgotpassword.html");
        string html = File.ReadAllText(filePath);
        string newOtpCode = GenerateOTP();
        html = html.Replace("{{otpcode}}", newOtpCode);
        await _emailService.SendEmail(request.Email, subject, html);
        return new ForgotPasswordQueryResponse();
    }

    private string GenerateOTP()
    {
        var random = new Random();
        return random.Next(1000, 9999).ToString();
    }
}
