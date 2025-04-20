using Domain.Entities;

namespace Infrastructure.Data.Interfaces;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetByEmailAsync(string email);
}
