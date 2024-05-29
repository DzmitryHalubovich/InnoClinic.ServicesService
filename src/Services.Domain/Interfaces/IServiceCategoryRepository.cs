using Services.Domain.Entities;

namespace Services.Domain.Interfaces;

public interface IServiceCategoryRepository
{
    public Task<ServiceCategory?> GetByIdAsync(int id);
}
