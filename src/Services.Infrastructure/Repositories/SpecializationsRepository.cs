using Microsoft.EntityFrameworkCore;
using Services.Contracts.Filtering;
using Services.Domain.Entities;
using Services.Domain.Interfaces;
using Services.Infrastructure.Data;

namespace Services.Infrastructure.Repositories;

public class SpecializationsRepository : ISpecializationsRepository
{
    private readonly ServicesDbContext _context;

    public SpecializationsRepository(ServicesDbContext context) =>
         _context = context;

    public async Task<List<Specialization>> GetAllAsync(SpecializationsQueryParameters queryParameters) =>
        await _context.Specializations.AsNoTracking()
            .IncludeServices(queryParameters.IncludeServices)
            .OnlyActive(queryParameters.OnlyActive)
            .ToListAsync();

    public async Task<List<Specialization>> GetAllWithServicesAsync() =>
        await _context.Specializations.AsNoTracking()
            .Include(x => x.Services)
            .ThenInclude(x => x.ServiceCategory)
            .ToListAsync();

    public async Task<Specialization?> GetByIdAsync(int id) =>
        await _context.Specializations.AsNoTracking()
            .Include(x => x.Services)
            .ThenInclude(x => x.ServiceCategory)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task CreateAsync(Specialization specialization)
    {
        _context.Specializations.Add(specialization);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Specialization specialization)
    {
        _context.Specializations.Update(specialization);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Specialization specialization)
    {
        _context.Specializations.Remove(specialization);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(Specialization specialization, bool statusIsChangedToInactive)
    {
        if (statusIsChangedToInactive)
        {
            foreach (var service in specialization.Services)
            {
                service.Status = Status.Inactive;
            }

            _context.Services.UpdateRange(specialization.Services);
        }

        _context.Specializations.Update(specialization);

        await _context.SaveChangesAsync();
    }
}
