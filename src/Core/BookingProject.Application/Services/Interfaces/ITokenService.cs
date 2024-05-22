// ITokenService.cs
using BookingProject.Application.Features.DTOs;
using BookingProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookingProject.Application.Services.Interfaces
{
	public interface ITokenService
	{
		Task<TokenDto> CreateToken(bool populateExp, AppUser user, HttpContext httpContext);
		Task<TokenDto> RefreshToken(AppUser user, HttpContext httpContext);
		ClaimsPrincipal GetPrincFromExpToken(string token);
	}
}
