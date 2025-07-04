using NotificationService.Models;
using Microsoft.EntityFrameworkCore;
namespace NotificationService.Data;
public class NotificationDbContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }
}