using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookingProject.MVC.Services;
using Microsoft.Extensions.Configuration;

public class PayPalService:IPayPalService
{
	private readonly HttpClient _httpClient;
	private readonly string _clientId;
	private readonly string _clientSecret;
	private readonly string _apiUrl;

	public PayPalService(IConfiguration configuration)
	{
		_httpClient = new HttpClient();
		_clientId = configuration["PayPal:ClientId"];
		_clientSecret = configuration["PayPal:ClientSecret"];
		_apiUrl = configuration["PayPal:ApiUrl"];
	}

	public async Task<string> GetAccessTokenAsync()
	{
		var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

		var response = await _httpClient.PostAsync($"{_apiUrl}/v1/oauth2/token", new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"));
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var tokenResponse = JsonSerializer.Deserialize<JsonElement>(json);
		return tokenResponse.GetProperty("access_token").GetString();
	}

	public async Task<string> CreateOrderAsync(decimal amount, string currency)
	{
		var accessToken = await GetAccessTokenAsync();
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		var order = new
		{
			intent = "CAPTURE",
			purchase_units = new[]
			{
				new
				{
					amount = new
					{
						currency_code = currency,
						value = amount.ToString("F2")
					}
				}
			},
			application_context = new
			{
				brand_name = "Your Brand",
				landing_page = "BILLING",
				cancel_url = "https://example.com/cancel",
				return_url = "https://example.com/return"
			}
		};

		var content = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync($"{_apiUrl}/v2/checkout/orders", content);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var orderResponse = JsonSerializer.Deserialize<JsonElement>(json);
		return orderResponse.GetProperty("id").GetString();
	}
}
