//using System;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;

//public class SaveApiCookiesMiddleware
//{
//	private readonly RequestDelegate _next;
//	private const string ApiUrl = "https://localhost:7197/"; // URL of your API

//	public SaveApiCookiesMiddleware(RequestDelegate next)
//	{
//		_next = next;
//	}

//	public async Task Invoke(HttpContext context, IHttpClientFactory clientFactory)
//	{
//		var response = context.Response;
//		var client = clientFactory.CreateClient();

//		// Process the request
//		await _next(context);

//		// If the response is from the API and contains cookies, save them to MVC's cookies
//		if (IsResponseFromApi(context.Request.Headers["Referer"]))
//		{
//			if (response.Headers.ContainsKey("Set-Cookie"))
//			{
//				var cookies = response.Headers["Set-Cookie"].ToList();

//				foreach (var cookie in cookies)
//				{
//					// Save API cookies to MVC application's cookies
//					response.Cookies.Append("api_" + Guid.NewGuid().ToString(), cookie, new CookieOptions
//					{
//						HttpOnly = true,
//						Secure = true,
//						// Set domain and path if necessary
//						// Domain = "",
//						// Path = "/"
//					});
//				}
//			}
//		}
//	}

//	private bool IsResponseFromApi(string refererHeader)
//	{
//		// Check if the Referer header matches the API URL
//		return !string.IsNullOrEmpty(refererHeader) && refererHeader.StartsWith(ApiUrl, StringComparison.OrdinalIgnoreCase);
//	}
//}
