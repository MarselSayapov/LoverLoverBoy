using Services.Models.Auth.Requests;
using Services.Models.Auth.Responses;

namespace Services.Interfaces;

public interface IAuthService
{
    public Task<AuthResponse> RegisterAsync(RegisterRequest requestDto);
    public Task<AuthResponse> LoginAsync(LoginRequest requestDto);
    public Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest requestDto);
}