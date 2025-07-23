namespace Services.Models.Auth.Requests;

public sealed record RefreshTokenRequest(string Token, string RefreshToken);