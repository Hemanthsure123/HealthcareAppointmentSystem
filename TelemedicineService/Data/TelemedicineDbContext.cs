using TelemedicineService.Models;
using Microsoft.EntityFrameworkCore;
namespace TelemedicineService.Data;
public class TelemedicineDbContext : DbContext
{
    public DbSet<VirtualAppointment> VirtualAppointments { get; set; }
    public TelemedicineDbContext(DbContextOptions<TelemedicineDbContext> options) : base(options) { }
}