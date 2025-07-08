using Services.Models.User.Responses;

namespace Services.Models.Auth.Responses;

public sealed record AuthResponse(string Token, string RefreshToken);