using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data.Repositories;

public class RepositoryBase<T>(ApplicationContext context) : IRepository<T>
    where T : EntityBase
{
    public virtual IQueryable<T> GetAll()
    {
        return context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public virtual async Task<EntityEntry<T>> CreateAsync(T entity)
    {
        var entry = context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entry;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }
}
