using BookingProject.Application.Features.DTOs;
using BookingProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BookingProject.Application.Services.Interfaces;

public interface ITokenService
{
	Task<TokenDto> CreateToken(bool populateExp, AppUser user,HttpContext httpContext);
	ClaimsPrincipal GetPrincFromExpToken(string token);
}
