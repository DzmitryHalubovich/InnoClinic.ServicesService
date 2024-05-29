using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;

namespace Services.Infrastructure.Data;

public class ServicesDbContext : DbContext
{
    public ServicesDbContext(DbContextOptions<ServicesDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServiceCategory>().HasData(
            new ServiceCategory() { Id = 1, Name = "Analyses", TimeSlotSize = TimeSpan.FromMinutes(20) },
            new ServiceCategory() { Id = 2, Name = "Consultation", TimeSlotSize = TimeSpan.FromMinutes(30) },
            new ServiceCategory() { Id = 3, Name = "Diagnostics", TimeSlotSize = TimeSpan.FromMinutes(60) });
    }

    public DbSet<Service> Services { get; set; }

    public DbSet<Specialization> Specializations { get; set; }

    public DbSet<ServiceCategory> ServiceCategories { get; set; }
}
