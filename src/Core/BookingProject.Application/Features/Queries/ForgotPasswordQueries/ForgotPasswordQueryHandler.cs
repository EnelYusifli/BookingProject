using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Caching.Memory;

namespace BookingProject.Application.Features.Queries;

public class ForgotPasswordQueryHandler : IRequestHandler<ForgotPasswordQueryRequest, ForgotPasswordQueryResponse>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;
	private readonly IUserRepository _userRepository;

	public ForgotPasswordQueryHandler(IEmailService emailService,UserManager<AppUser> userManager,IUserRepository userRepository)
    {
        _emailService = emailService;
        _userManager = userManager;
		_userRepository = userRepository;
	}
    public async Task<ForgotPasswordQueryResponse> Handle(ForgotPasswordQueryRequest request, CancellationToken cancellationToken)
    {
        var user=await _userManager.FindByEmailAsync(request.Email);
        if (user is null) throw new NotFoundException("No account found with this email address.");
        string subject = "Reset Password";
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email", "forgotpassword.html");
        string html = File.ReadAllText(filePath);
        string token =await _userManager.GeneratePasswordResetTokenAsync(user);
        user.PasswordResetToken = token;
        user.ResetTokenExpires= DateTime.Now.AddMinutes(5);
        token = $"https://localhost:7183/account/resetpassword?token={token}";
        html = html.Replace("{{token}}", token);
        await _userRepository.CommitAsync();
        await _emailService.SendEmail(request.Email, subject, html);
        return new ForgotPasswordQueryResponse();
    }
}
