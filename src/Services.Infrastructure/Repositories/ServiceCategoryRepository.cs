using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Interfaces;
using Services.Infrastructure.Data;

namespace Services.Infrastructure.Repositories;

public class ServiceCategoryRepository : IServiceCategoryRepository
{
    private readonly ServicesDbContext _servicesDbContext;

    public ServiceCategoryRepository(ServicesDbContext servicesDbContext) =>
        _servicesDbContext = servicesDbContext;

    public async Task<ServiceCategory?> GetByIdAsync(int id) =>
        await _servicesDbContext.ServiceCategories.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id.Equals(id));
}
