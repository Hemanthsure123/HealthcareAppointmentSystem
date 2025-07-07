using Microsoft.AspNetCore.Mvc;
using AppointmentService.Services;

namespace AppointmentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FhirTestController : ControllerBase
{
    private readonly FhirClientService _fhirClientService;

    public FhirTestController(FhirClientService fhirClientService)
    {
        _fhirClientService = fhirClientService;
    }

    [HttpGet("patient/{id}")]
    public async Task<IActionResult> GetPatient(string id)
    {
        try
        {
            var result = await _fhirClientService.GetPatientAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}