//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;
//using System.Threading.Tasks;
//using BookingProject.Application.Services.Interfaces;
//using Microsoft.AspNetCore.Identity;
//using BookingProject.Domain.Entities;

//public class BaseController : Controller
//{
//	protected readonly ITokenService _tokenService;
//	protected readonly IHttpContextAccessor _httpContextAccessor;
//	private readonly UserManager<AppUser> _userManager;

//	public BaseController(ITokenService tokenService, IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager)
//	{
//		_tokenService = tokenService;
//		_httpContextAccessor = httpContextAccessor;
//		_userManager = userManager;
//	}

//	protected async Task RefreshTokenIfNeeded()
//	{
//		var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

//		// Check if refresh token is present
//		if (!string.IsNullOrEmpty(refreshToken))
//		{
//			// Retrieve current user
//			var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

//			// Refresh tokens if necessary
//			if (IsTokenExpired())
//			{
//				// Refresh tokens
//				var tokenResponse = await _tokenService.RefreshToken(user, _httpContextAccessor.HttpContext);

//				// Update the access token in the HTTP context
//				_httpContextAccessor.HttpContext.Response.Cookies.Append("token", tokenResponse.AccessToken, new CookieOptions
//				{
//					Expires = DateTime.UtcNow.AddMinutes(10),
//					HttpOnly = true,
//					Secure = true,
//					IsEssential = true,
//					SameSite = SameSiteMode.None
//				});
//			}
//		}
//	}

//	private bool IsTokenExpired()
//	{
//		// Implement logic to check if access token is expired
//		// This might involve decoding the token and checking the expiry date
//		// Return true if the access token is expired, false otherwise
//		return true;
//	}
//}
