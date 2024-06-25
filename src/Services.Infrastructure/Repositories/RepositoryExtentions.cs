using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;

namespace Services.Infrastructure.Repositories;

public static class RepositoryExtentions
{
    public static IQueryable<Specialization> IncludeServices(this IQueryable<Specialization> specializations,
        bool includeServices) => includeServices
            ? specializations.Include(x => x.Services)
                .ThenInclude(s => s.ServiceCategory)
            : specializations;

    public static IQueryable<Specialization> OnlyActive(this IQueryable<Specialization> specializations,
        bool onlyActive) => onlyActive
            ? specializations.Where(x => x.Status.Equals(Status.Active))
            : specializations;
}
