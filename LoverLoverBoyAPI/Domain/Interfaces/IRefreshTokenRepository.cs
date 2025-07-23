using Domain.Entities;

namespace Domain.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    public Task<RefreshToken?> GetByTokenAsync(string token);
}