using Domain.Entities;
using Services.Models.Auth.Responses;

namespace Services.Interfaces;

public interface IJwtService
{
    public string GenerateToken(string login, string email, Guid userId);
    public Task<(string token, string refreshToken)> GetRefreshTokenAsync(string token, string refreshToken);
    public Task<(string token, string refreshToken)> GetNewAccessTokenWithRefreshAsync(User user);
}