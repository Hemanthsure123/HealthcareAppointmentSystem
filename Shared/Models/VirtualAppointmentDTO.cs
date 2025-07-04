using System;

namespace Shared.Models;
public class VirtualAppointmentDTO
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string CallLink { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Active, Ended
}