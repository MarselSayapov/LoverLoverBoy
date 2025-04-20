namespace Services.Models.Auth.Requests;

public record RegisterRequest(string Login, string Email, string Password);