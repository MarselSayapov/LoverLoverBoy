using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Domain.Interfaces;

public interface IRepository<T> where T : EntityBase
{
    public IQueryable<T> GetAll();
    public Task<T?> GetByIdAsync(Guid id);
    public Task<T> CreateAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
}