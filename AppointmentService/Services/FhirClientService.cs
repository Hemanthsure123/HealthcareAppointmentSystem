using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace AppointmentService.Services;
public class FhirClientService
{
    private readonly HttpClient _httpClient;
    public FhirClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://healthcare-appointment-system-fhir.fhir.azurehealthcareapis.com");
    }
    public async Task<string> GetPatientAsync(string patientId)
    {
        var response = await _httpClient.GetAsync($"Patient/{patientId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}