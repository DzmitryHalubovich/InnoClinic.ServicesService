using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Interfaces;
using Services.Infrastructure.Data;

namespace Services.Infrastructure.Repositories;

public class ServicesRepository : IServicesRepository
{
    private readonly ServicesDbContext _context;

    public ServicesRepository(ServicesDbContext context) =>
        _context = context;

    public async Task<List<Service>> GetAllAsync() =>
        await _context.Services.AsNoTracking()
            .Include(x => x.ServiceCategory)
            .ToListAsync();

    public async Task<Service?> GetByIdAsync(int id) =>
        await _context.Services.AsNoTracking()
            .Include(x => x.ServiceCategory)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task CreateAsync(Service service)
    {
        _context.Services.Add(service);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Service service)
    {
        _context.Services.Update(service);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Service service)
    {
        _context.Services.Remove(service);

        await _context.SaveChangesAsync();
    }
}
