using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Infrastructure.Configuration;

namespace Services.Infrastructure.Data;

public class ServicesDbContext : DbContext
{
    public ServicesDbContext(DbContextOptions<ServicesDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ServiceCategoryConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Service> Services { get; set; }

    public DbSet<Specialization> Specializations { get; set; }

    public DbSet<ServiceCategory> ServiceCategories { get; set; }
}
