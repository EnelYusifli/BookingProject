//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;

//public class CustomHttpClientHandler : DelegatingHandler
//{
//	private readonly IHttpContextAccessor _httpContextAccessor;

//	public CustomHttpClientHandler(IHttpContextAccessor httpContextAccessor)
//	{
//		_httpContextAccessor = httpContextAccessor;
//	}

//	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//	{
//		var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["token"];
//		if (!string.IsNullOrEmpty(accessToken))
//		{
//			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
//		}

//		return await base.SendAsync(request, cancellationToken);
//	}
//}
