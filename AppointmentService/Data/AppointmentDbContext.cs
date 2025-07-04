using AppointmentService.Models;
using Microsoft.EntityFrameworkCore;
namespace AppointmentService.Data;
public class AppointmentDbContext : DbContext
{
	public DbSet<Appointment> Appointments { get; set; }
	public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options) { }
}