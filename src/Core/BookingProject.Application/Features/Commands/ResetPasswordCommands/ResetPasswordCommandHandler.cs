using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.ResetPasswordCommands;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SmtpServer.Text;

namespace BookingProject.Application.Features.Commands;


public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommandRequest, ResetPasswordCommandResponse>
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IUserRepository _userRepository;

	public ResetPasswordCommandHandler(UserManager<AppUser> userManager,IUserRepository userRepository)
	{
		_userManager = userManager;
		_userRepository = userRepository;
	}

	public async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
	{
		var passwordRequirements = _userManager.Options.Password;

		if (request.Password.Length < passwordRequirements.RequiredLength)
		{
			throw new BadRequestException($"Password must be at least {passwordRequirements.RequiredLength} characters long");
		}

		//if (passwordRequirements.RequireNonAlphanumeric && !request.Password.Any(char.IsSymbol))
		//{
		//    throw new BadRequestException("Password must contain at least one non-alphanumeric character");
		//}
		if (passwordRequirements.RequireNonAlphanumeric)
		{
			bool hasNonAlphanumeric = false;
			foreach (char c in request.Password)
			{
				if (!char.IsLetterOrDigit(c))
				{
					hasNonAlphanumeric = true;
					break;
				}
			}

			if (!hasNonAlphanumeric)
			{
				throw new BadRequestException("Password must contain at least one non-alphanumeric character");
			}
		}


		if (passwordRequirements.RequireDigit && !request.Password.Any(char.IsDigit))
		{
			throw new BadRequestException("Password must contain at least one digit");
		}

		if (passwordRequirements.RequireLowercase && !request.Password.Any(char.IsLower))
		{
			throw new BadRequestException("Password must contain at least one lowercase letter");
		}

		if (passwordRequirements.RequireUppercase && !request.Password.Any(char.IsUpper))
		{
			throw new BadRequestException("Password must contain at least one uppercase letter");
		}
		request.Token = request.Token.Replace(' ', '+');
		AppUser user = await _userManager.Users.Where(x => x.PasswordResetToken == request.Token).FirstOrDefaultAsync();
		if (user is null)
			throw new NotFoundException("User not found");
		if (user.ResetTokenExpires < DateTime.Now)
			throw new BadRequestException("Token expired");
		var resetToken=await _userManager.GeneratePasswordResetTokenAsync(user);
		var result = await _userManager.ResetPasswordAsync(user, resetToken, request.Password);
		if (result.Succeeded)
		{
			user.PasswordResetToken = null;
			user.ResetTokenExpires = null;
			await _userRepository.CommitAsync();
			return new ResetPasswordCommandResponse();

		}
		else throw new BadRequestException("Password could not be reseted");
	}
}
