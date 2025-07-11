using System;

namespace Shared.Models;
public class DoctorAvailabilityDTO { public Guid Id { get; set; } public string DoctorId { get; set; } = string.Empty; public DayOfWeek Day { get; set; } public TimeSpan StartTime { get; set; } public TimeSpan EndTime { get; set; } }