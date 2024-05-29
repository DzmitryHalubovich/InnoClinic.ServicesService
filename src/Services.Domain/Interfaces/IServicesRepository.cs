using Services.Domain.Entities;

namespace Services.Domain.Interfaces;

public interface IServicesRepository
{
    public Task<List<Service>> GetAllAsync();

    public Task<Service?> GetByIdAsync(Guid id);

    public Task CreateAsync(Service service);

    public Task UpdateAsync(Service service);

    public Task DeleteAsync(Service service);
}
