using Services.Contracts.Filtering;
using Services.Domain.Entities;

namespace Services.Domain.Interfaces;

public interface ISpecializationsRepository
{
    public Task<List<Specialization>> GetAllAsync(SpecializationsQueryParameters queryParameters);

    public Task<Specialization?> GetByIdAsync(int id);

    public Task CreateAsync(Specialization specialization);

    public Task UpdateAsync(Specialization specialization);

    public Task UpdateStatusAsync(Specialization specialization, bool statusIsChangedToInactive);

    public Task DeleteAsync(Specialization specialization);
}
