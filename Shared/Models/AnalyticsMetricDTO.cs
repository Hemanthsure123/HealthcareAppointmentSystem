using System;

namespace Shared.Models;
public class AnalyticsMetricDTO
{
    public Guid Id { get; set; }
    public string MetricType { get; set; } = string.Empty; // e.g., AppointmentCount, NoShowRate
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}