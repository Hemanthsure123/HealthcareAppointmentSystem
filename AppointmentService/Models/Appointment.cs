using Shared.Models;
using System;
namespace AppointmentService.Models;
public class Appointment
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime Date { get; set; }
    public bool IsVirtual { get; set; }
    public string Status { get; set; } = "Scheduled";
}