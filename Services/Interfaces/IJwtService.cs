namespace Services.Interfaces;

public interface IJwtService
{
    public string GenerateToken(string login, string email, Guid userId);
    public string GenerateRefreshToken(string login);
}