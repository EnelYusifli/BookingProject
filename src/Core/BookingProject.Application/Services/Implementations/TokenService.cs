// TokenService.cs
using BookingProject.Application.Features.DTOs;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookingProject.Application.Services.Implementations
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;
		private readonly UserManager<AppUser> _userManager;

		public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
		{
			_configuration = configuration;
			_userManager = userManager;
		}

		public async Task<TokenDto> CreateToken(bool populateExp, AppUser user, HttpContext httpContext)
		{
			var userRoles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email)
			};

			claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var date = DateTime.UtcNow;
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = date.AddMinutes(10),
				SigningCredentials = credentials,
				Issuer = _configuration["JWT:Issuer"],
				Audience = _configuration["JWT:Audience"],
				NotBefore = date
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var encryptedToken = tokenHandler.WriteToken(token);

			var refreshToken = GenerateRefreshToken();
			user.RefreshToken = refreshToken;

			httpContext.Response.Cookies.Append("token", encryptedToken, new CookieOptions
			{
				Expires = DateTime.UtcNow.AddMinutes(10),
				HttpOnly = true,
				Secure = true,
				IsEssential = true,
				SameSite = SameSiteMode.None
			});

			httpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
			{
				Expires = DateTime.UtcNow.AddDays(7),
				HttpOnly = true,
				Secure = true,
				IsEssential = true,
				SameSite = SameSiteMode.None
			});

			if (populateExp)
				user.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);

			await _userManager.UpdateAsync(user);

			return new TokenDto(encryptedToken, refreshToken);
		}

		public async Task<TokenDto> RefreshToken(AppUser user, HttpContext httpContext)
		{
			var newTokens = await CreateToken(true, user, httpContext);

			return newTokens;
		}

		private string GenerateRefreshToken()
		{
			byte[] bytes = new byte[64];
			using (var randomNumber = RandomNumberGenerator.Create())
			{
				randomNumber.GetBytes(bytes);
				return Convert.ToBase64String(bytes);
			}
		}

		public ClaimsPrincipal GetPrincFromExpToken(string token)
		{
			var jwtSettings = _configuration.GetSection("JWT");
			var secretKey = jwtSettings["SecurityKey"];

			var tokenValidationParams = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = false,
				ValidIssuer = jwtSettings["Issuer"],
				ValidAudience = jwtSettings["Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out SecurityToken securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;

			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}
	}
}
