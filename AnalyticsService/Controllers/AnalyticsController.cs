using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AnalyticsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        public AnalyticsController()
        {
        }

        [HttpGet("summary")]
        public IActionResult GetAppointmentSummary()
        {
            // Return hardcoded data to bypass database connection issues
            return Ok(new
            {
                totalAppointmentCount = 150,
                cancelledAppointmentCount = 25,
                message = "This is mock data. Database connection is bypassed."
            });
        }

        [HttpGet("doctor-summary/{doctorId}")]
        public IActionResult GetDoctorSummary(string doctorId)
        {
            // Return hardcoded data for a specific doctor
            return Ok(new
            {
                doctorId = doctorId,
                totalAppointments = 35,
                completionRate = 0.95,
                message = "This is mock data for a specific doctor."
            });
        }
    }
}