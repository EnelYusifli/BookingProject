// TokenRefreshMiddleware.cs
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

public class TokenRefreshMiddleware
{
	private readonly RequestDelegate _next;
	private readonly IServiceProvider _serviceProvider;

	public TokenRefreshMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
	{
		_next = next;
		_serviceProvider = serviceProvider;
	}

	public async Task InvokeAsync(HttpContext context, ITokenService tokenService, UserManager<AppUser> userManager)
	{
		var accessToken = context.Request.Cookies["token"];
		var refreshToken = context.Request.Cookies["refreshToken"];

		if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
		{
			try
			{
				var principal = tokenService.GetPrincFromExpToken(accessToken);
				var user = await userManager.FindByNameAsync(principal.Identity.Name);

				if (user != null && user.RefreshToken == refreshToken && user.RefreshTokenExpires > DateTime.UtcNow)
				{
					var newTokens = await tokenService.RefreshToken(user, context);

					if (newTokens.AccessToken != accessToken)
					{
						context.Response.Cookies.Append("token", newTokens.AccessToken, new CookieOptions
						{
							HttpOnly = true,
							Secure = true,
							Expires = DateTime.UtcNow.AddMinutes(10)
						});

						context.Response.Cookies.Append("refreshToken", newTokens.RefreshToken, new CookieOptions
						{
							HttpOnly = true,
							Secure = true,
							Expires = DateTime.UtcNow.AddDays(7)
						});
					}
				}
			}
			catch (SecurityTokenException)
			{
				// Token validation failed, continue with the request without refreshing
			}
			catch (Exception ex)
			{
				// Log the exception
			}
		}

		await _next(context);
	}
}
