using System;

namespace Shared.Models;
public class AuditLogDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Action { get; set; } = string.Empty; // e.g., ViewPatient, UpdateAppointment
    public string Entity { get; set; } = string.Empty; // e.g., Patient, Appointment
    public DateTime Timestamp { get; set; }
}