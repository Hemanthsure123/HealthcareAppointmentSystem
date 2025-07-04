using Shared.Models;
using System;
namespace TelemedicineService.Models;
public class VirtualAppointment
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string CallLink { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
}