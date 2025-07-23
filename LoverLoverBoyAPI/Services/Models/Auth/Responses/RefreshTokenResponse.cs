namespace Services.Models.Auth.Responses;

public sealed record RefreshTokenResponse(string Token, string RefreshToken);