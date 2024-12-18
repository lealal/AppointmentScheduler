using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TestItem> TestItems { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
