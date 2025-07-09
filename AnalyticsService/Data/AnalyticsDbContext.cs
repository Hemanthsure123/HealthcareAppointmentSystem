using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace AnalyticsService.Data;

public class AnalyticsDbContext : DbContext
{
    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options) { }

    // This represents the Appointments table from your database
    public DbSet<AppointmentDTO> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // This line tells EF Core that the AppointmentDTO model maps to the "Appointments" table
        modelBuilder.Entity<AppointmentDTO>().ToTable("Appointments");
    }
}