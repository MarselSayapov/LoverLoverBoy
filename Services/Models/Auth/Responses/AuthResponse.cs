using Services.Models.User.Responses;

namespace Services.Models.Auth.Responses;

public class AuthResponse
{
    public string Token { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public UserResponse User { get; set; } = null!;
}