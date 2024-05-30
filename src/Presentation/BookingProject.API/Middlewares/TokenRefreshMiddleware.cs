//// TokenRefreshMiddleware.cs
//using BookingProject.Application.Services.Interfaces;
//using BookingProject.Domain.Entities;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Threading.Tasks;

//public class TokenRefreshMiddleware
//{
//	private readonly RequestDelegate _next;
//	private readonly IServiceProvider _serviceProvider;

//	public TokenRefreshMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
//	{
//		_next = next;
//		_serviceProvider = serviceProvider;
//	}

//	public async Task InvokeAsync(HttpContext context, ITokenService tokenService, UserManager<AppUser> userManager)
//	{
//		var accessToken = context.Request.Cookies["token"];
//		var refreshToken = context.Request.Cookies["refreshToken"];

//		if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
//		{
//			try
//			{
//				var principal = tokenService.GetPrincFromExpToken(accessToken);
//				var user = await userManager.FindByNameAsync(principal.Identity.Name);

//				if (user != null && user.RefreshTokenExpires > DateTime.UtcNow)
//				{
//					var newTokens = await tokenService.RefreshToken(user, context);

//					if (newTokens.AccessToken != accessToken)
//					{
//						context.Response.Cookies.Append("token", newTokens.AccessToken, new CookieOptions
//						{
//							Expires = DateTime.UtcNow.AddMinutes(10),
//							HttpOnly = true,
//							Secure = true,
//							IsEssential = true,
//							SameSite = SameSiteMode.None
//						});

//						context.Response.Cookies.Append("refreshToken", newTokens.RefreshToken, new CookieOptions
//						{
//							Expires = DateTime.UtcNow.AddDays(7),
//							HttpOnly = true,
//							Secure = true,
//							IsEssential = true,
//							SameSite = SameSiteMode.None
//						});
//					}
//				}
//			}
//			catch (SecurityTokenException ex)
//			{
//				Console.WriteLine(ex.Message);

//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine(ex.Message);
//			}
//		}

//		await _next(context);
//	}
//}
