using Shared.Models;
using AppointmentService.Models;
using Microsoft.EntityFrameworkCore;
namespace AppointmentService.Data;


public class AppointmentDbContext : DbContext
{
	public DbSet<Appointment> Appointments { get; set; }
	public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options) { }
    public DbSet<HospitalDTO> Hospitals { get; set; }
    public DbSet<SpecialtyDTO> Specialties { get; set; }
    public DbSet<DoctorAvailabilityDTO> DoctorAvailabilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<HospitalDTO>().ToTable("Hospitals");
        modelBuilder.Entity<SpecialtyDTO>().ToTable("Specialties");
        modelBuilder.Entity<DoctorAvailabilityDTO>().ToTable("DoctorAvailabilities");
    }
}

