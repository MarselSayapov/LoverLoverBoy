namespace Services.Models.Auth.Requests;

public sealed record RegisterRequest(string Login, string Email, string Password);