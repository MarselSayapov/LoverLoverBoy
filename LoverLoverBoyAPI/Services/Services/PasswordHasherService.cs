using System.Security.Cryptography;
using System.Text;
using Services.Interfaces;

namespace Services.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var computedHash = HashPassword(password);
        return computedHash == hashedPassword;
    }
}