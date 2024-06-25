using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;

namespace Services.Infrastructure.Configuration;

public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder.HasData(
            new ServiceCategory() { Id = 1, Name = "Analyses", TimeSlotSize = TimeSpan.FromMinutes(20) },
            new ServiceCategory() { Id = 2, Name = "Consultation", TimeSlotSize = TimeSpan.FromMinutes(30) },
            new ServiceCategory() { Id = 3, Name = "Diagnostics", TimeSlotSize = TimeSpan.FromMinutes(60) });
    }
}
