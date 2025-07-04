using AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;
namespace AnalyticsService.Data;
public class AnalyticsDbContext : DbContext
{
    public DbSet<AnalyticsMetric> AnalyticsMetrics { get; set; }
    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options) { }
}