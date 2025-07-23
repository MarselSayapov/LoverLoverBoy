using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;

namespace Services.Services;

public class JwtService(IOptions<AuthOptions> options, IUnitOfWork unitOfWork) : IJwtService
{
    public string GenerateToken(string login, string email, Guid userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            options.Value.Issuer,
            options.Value.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: cred);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(string token, string refreshToken)> GetRefreshTokenAsync(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(token, GetValidationParameters());

        if (validatedToken is null)
        {
            throw new BadRequestException("Invalid token");
        }
        
        var jti = validatedToken.Claims
            .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;

        if (string.IsNullOrEmpty(jti))
        {
            throw new BadRequestException("Invalid token");
        }
        
        var storedRefreshToken = await unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);

        if (storedRefreshToken is null)
        {
            throw new BadRequestException("This refresh token does not exist");
        }

        if (DateTime.UtcNow > storedRefreshToken.ExpiresAt)
        {
            throw new BadRequestException("This refresh token has expired");
        }

        if (storedRefreshToken.Invalidated)
        {
            throw new BadRequestException("This refresh token has been invalidated");
        }
        
        var userId = validatedToken.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            throw new BadRequestException("Current user is not found");
        }
        
        var user = await unitOfWork.Users.GetByIdAsync(Guid.Parse(userId));

        if (user is null)
        {
            throw new NotFoundException("This user does not exist");
        }
        
        return await GenerateJwtAndRefreshTokenAsync(user, refreshToken);
    }
    
    public Task<(string token, string refreshToken)> GetNewAccessTokenWithRefreshAsync(User user)
    {
        return GenerateJwtAndRefreshTokenAsync(user, null);
    }

    private static ClaimsPrincipal? GetPrincipalFromToken(string token,
        TokenValidationParameters parameters)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var tokenParameters = parameters.Clone();
            tokenParameters.ValidateLifetime = false;
            var principal = tokenHandler.ValidateToken(token, tokenParameters, out var validatedToken);
            return IsJwtWithValidSecurityAlgorithm(validatedToken) ? principal : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken token) => token is JwtSecurityToken jwtSecurityToken &&
        jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase);

    private async Task<(string token, string refreshToken)> GenerateJwtAndRefreshTokenAsync(User user,
        string? existingRefreshToken)
    {
        var token = GenerateToken(user.Login, user.Email, user.Id);
        var refreshToken = await GenerateRefreshTokenAsync(token, user, existingRefreshToken);
        
        return (token, refreshToken);
    }

    private async Task<string> GenerateRefreshTokenAsync(string token, User user, string? existingRefreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var jti = jwtToken.Id;

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            JwtId = jti,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (!string.IsNullOrEmpty(existingRefreshToken))
        {
            var existingToken = await unitOfWork.RefreshTokens.GetByTokenAsync(existingRefreshToken);

            if (existingToken is not null)
            {
                await unitOfWork.RefreshTokens.DeleteAsync(existingToken);
            }
        }
        
        await unitOfWork.RefreshTokens.CreateAsync(refreshToken);
        
        return refreshToken.Token;
    }
    
    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = options.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = options.Value.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key)),
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true
        };
    }
}