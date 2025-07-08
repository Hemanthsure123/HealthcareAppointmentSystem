using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AppointmentService.Services;

public class FhirClientService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public FhirClientService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri("https://healthcareappointment-fhir-service.fhir.azurehealthcareapis.com");
    }

    private async Task<string> GetFhirAccessTokenAsync()
    {
        using var tokenClient = new HttpClient();
        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{_configuration["AzureAd:TenantId"]}/oauth2/v2.0/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _configuration["AzureAd:ClientId"],
                ["client_secret"] = _configuration["AzureAd:ClientSecret"],
                ["scope"] = "https://healthcareappointment-fhir-service.fhir.azurehealthcareapis.com/.default"
            })
        };

        var tokenResponse = await tokenClient.SendAsync(tokenRequest);
        tokenResponse.EnsureSuccessStatusCode();

        var jsonResponse = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();
        return jsonResponse.GetProperty("access_token").GetString();
    }

    public async Task<JsonElement> CreateResourceAsync(string resourceType, string jsonContent)
    {
        var accessToken = await GetFhirAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var content = new StringContent(jsonContent, Encoding.UTF8, "application/fhir+json");
        var response = await _httpClient.PostAsync(resourceType, content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<JsonElement>(responseString);
    }
}