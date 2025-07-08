using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Contexts;

namespace Infrastructure.Data.Repositories;

public class RefreshTokenRepository(ApplicationContext context) : RepositoryBase<RefreshToken>(context), IRefreshTokenRepository
{
    private readonly ApplicationContext _context = context;

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens.FindAsync(token);
    }
}