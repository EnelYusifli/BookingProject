// Custom middleware to save API response cookies to MVC application's cookies
using Microsoft.Net.Http.Headers;

public class SaveApiCookiesMiddleware
{
	private readonly RequestDelegate _next;

	public SaveApiCookiesMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		var originalBodyStream = context.Response.Body;
		using (var responseBody = new MemoryStream())
		{
			context.Response.Body = responseBody;

			// Continue processing the request
			await _next(context);

			// After processing the response, read and save cookies
			if (context.Response.Headers.TryGetValue("Set-Cookie", out var cookies))
			{
				foreach (var cookie in cookies)
				{
					var parsedCookie = SetCookieHeaderValue.Parse(cookie);
					// Save each cookie to MVC application's response
					context.Response.Cookies.Append("API_" + parsedCookie.Name.ToString(), parsedCookie.Value.ToString(),
						new CookieOptions
						{
							// Adjust options as needed
							Expires = parsedCookie.Expires ?? DateTimeOffset.MinValue,
							HttpOnly = parsedCookie.HttpOnly,
							Secure = parsedCookie.Secure,
							SameSite = (Microsoft.AspNetCore.Http.SameSiteMode)(int)parsedCookie.SameSite // Explicit conversion
						});
				}
			}

			// Reset the response body to the original stream
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			await responseBody.CopyToAsync(originalBodyStream);
		}
	}
}

// Extension method used to add the middleware to the HTTP request pipeline
public static class SaveApiCookiesMiddlewareExtensions
{
	public static IApplicationBuilder UseSaveApiCookiesMiddleware(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<SaveApiCookiesMiddleware>();
	}
}
