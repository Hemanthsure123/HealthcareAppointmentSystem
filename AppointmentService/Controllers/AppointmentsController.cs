using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AppointmentService.Data;
using AppointmentService.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using AppointmentService.Services;
using Microsoft.AspNetCore.SignalR;
using AppointmentService.Hubs;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protect all endpoints in this controller
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentDbContext _context;
        private readonly FhirClientService _fhirClient;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AppointmentsController(AppointmentDbContext context, FhirClientService fhirClient, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _fhirClient = fhirClient;
            _hubContext = hubContext;
        }

        // GET: api/appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsForUser()
        {
            // In a real app, you would get the user's ID from the token and filter.
            // For now, we return all appointments.
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/appointments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }
            return appointment;
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
        {
            // 1. Save to SQL Database first
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // 2. Create and save a FHIR version of the Appointment
            try
            {
                var fhirAppointment = new
                {
                    resourceType = "Appointment",
                    status = "booked",
                    start = appointment.Date,
                    end = appointment.Date.AddMinutes(30), // Assuming a 30-minute appointment
                    participant = new[]
                    {
                        new { actor = new { reference = $"Patient/{appointment.PatientId}" }, status = "accepted" },
                        new { actor = new { reference = $"Practitioner/{appointment.DoctorId}" }, status = "accepted" }
                    }
                };

                string fhirJson = JsonSerializer.Serialize(fhirAppointment);


                var createdFhirAppointment = await _fhirClient.CreateResourceAsync("Appointment", fhirJson);

                appointment.FhirId = createdFhirAppointment.GetProperty("id").GetString();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"SQL record created, but failed to create FHIR record: {ex.Message}");
            }

            // 3. Send a real-time notification
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Admin", $"New appointment created for patient {appointment.PatientId}");

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        // PUT: api/appointments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(Guid id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Appointments.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/appointments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // We'll update the status instead of deleting the record
            appointment.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}