using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.DTOs;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands
{
    public class AuthLoginCommandHandler : IRequestHandler<AuthLoginCommandRequest, AuthLoginCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ITokenService _tokenService;

		public AuthLoginCommandHandler(
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                IConfiguration configuration,
				IHttpContextAccessor httpContextAccessor,
                ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
			_tokenService = tokenService;
		}

        public async Task<AuthLoginCommandResponse> Handle(AuthLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null) 
                user = await _userManager.FindByEmailAsync(request.UserName);

			if (user is null)
            {
                throw new BadRequestException("Invalid credentials. Please try again.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            if (!result.Succeeded)
            {
                throw new BadRequestException("Invalid credentials. Please try again.");
            }

            TokenDto dto = await _tokenService.CreateToken(true,user, _httpContextAccessor.HttpContext);

            return new AuthLoginCommandResponse()
            {
                UserName = user.UserName,
                Token = dto.accessToken,
                RefreshToken = dto.refreshToken,
            };
        }
	}
}
