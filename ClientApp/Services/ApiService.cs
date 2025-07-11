using Microsoft.Extensions.Configuration;
using Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ClientApp.Services;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    // Logs in a user and returns the token
    public async Task<string?> LoginAsync(object loginModel)
    {
        var gatewayUrl = _configuration["ApiService:BaseUrl"];
        var loginClient = _httpClientFactory.CreateClient();
        var jsonContent = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");

        var response = await loginClient.PostAsync($"{gatewayUrl}/api/auth/login", jsonContent);

        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
        return jsonResponse.GetProperty("token").GetString();
    }

    // Gets the token from the user's session cookie
    private string? GetTokenFromCookie()
    {
        // This is a simplified way to get the token for API calls
        // In a real app, you would manage this more robustly.
        var tokenClaim = _httpContextAccessor.HttpContext?.User.FindFirst("jwt");
        return tokenClaim?.Value;
    }

    public async Task<IEnumerable<AppointmentDTO>?> GetAppointmentsAsync()
    {
        var token = GetTokenFromCookie();
        if (string.IsNullOrEmpty(token)) return new List<AppointmentDTO>();

        var apiClient = _httpClientFactory.CreateClient();
        apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var gatewayUrl = _configuration["ApiService:BaseUrl"];
        var response = await apiClient.GetAsync($"{gatewayUrl}/api/appointments");

        if (!response.IsSuccessStatusCode) return new List<AppointmentDTO>();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<AppointmentDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task CreateAppointmentAsync(AppointmentDTO appointment)
    {
        var token = GetTokenFromCookie();
        var apiClient = _httpClientFactory.CreateClient();
        apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var gatewayUrl = _configuration["ApiService:BaseUrl"];
        var content = new StringContent(JsonSerializer.Serialize(appointment), Encoding.UTF8, "application/json");
        var response = await apiClient.PostAsync($"{gatewayUrl}/api/appointments", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<JsonElement> GetAcsTokenAsync(string appointmentId)
    {
        var token = GetTokenFromCookie();
        var apiClient = _httpClientFactory.CreateClient();
        apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var gatewayUrl = _configuration["ApiService:BaseUrl"];
        var response = await apiClient.PostAsync($"{gatewayUrl}/api/telemedicine/token/{appointmentId}", null);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
        return jsonResponse;
    }

    public async Task<HttpResponseMessage> RegisterUserAsync(object registrationModel)
    {
        var gatewayUrl = _configuration["ApiService:BaseUrl"];
        var registerClient = _httpClientFactory.CreateClient();
        var jsonContent = new StringContent(JsonSerializer.Serialize(registrationModel), Encoding.UTF8, "application/json");

        var response = await registerClient.PostAsync($"{gatewayUrl}/api/auth/register", jsonContent);
        return response;
    }

    public async Task<IEnumerable<UserDTO>?> GetPendingUsersAsync()
    {
        var token = GetTokenFromCookie();
        var apiClient = _httpClientFactory.CreateClient();
        apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var gatewayUrl = _configuration["ApiService:BaseUrl"];
        var response = await apiClient.GetAsync($"{gatewayUrl}/api/admin/pending-users");
        if (!response.IsSuccessStatusCode) return new List<UserDTO>();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<UserDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task ApproveUserAsync(string userId)
    {
        var token = GetTokenFromCookie();
        var apiClient = _httpClientFactory.CreateClient();
        apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var gatewayUrl = _configuration["ApiService:BaseUrl"];
        var response = await apiClient.PostAsync($"{gatewayUrl}/api/admin/approve-user/{userId}", null);
        response.EnsureSuccessStatusCode();
    }
}