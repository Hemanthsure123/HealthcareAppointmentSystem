using Shared.Models;
using System;
namespace NotificationService.Models;
public class AuditLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}