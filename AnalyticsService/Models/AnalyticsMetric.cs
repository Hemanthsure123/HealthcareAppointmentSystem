using Shared.Models;
using System;
namespace AnalyticsService.Models;
public class AnalyticsMetric
{
    public Guid Id { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}