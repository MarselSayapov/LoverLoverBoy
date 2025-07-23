using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Models.Auth.Requests;
using Services.Models.Auth.Responses;

namespace Services.Services;

public class AuthService(
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    IPasswordHasherService passwordHasherService,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest requestDto)
    {
        try
        {
            if (unitOfWork.Users.GetAll().Any(user => user.Email == requestDto.Email))
            {
                throw new DuplicateException("Пользователь с таким Email уже существует.");
            }

            var hashedPassword = passwordHasherService.HashPassword(requestDto.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = requestDto.Email,
                Login = requestDto.Login,
                PasswordHash = hashedPassword,
            };
            await unitOfWork.Users.CreateAsync(user);

            var (token, refreshToken) = await jwtService.GetNewAccessTokenWithRefreshAsync(user);
            return new AuthResponse(token, refreshToken);

        }
        catch (DuplicateException)
        {
            logger.LogError("Не удалось создать пользователя с Email: {}, такой пользователь уже существует.",
                requestDto.Email);
            throw;
        }
        catch (Exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при поиске пользователя\n\tEmail: {}, Login: {}",
                requestDto.Email, requestDto.Login);
            throw;
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest requestDto)
    {
        var user = await unitOfWork.Users.GetAll().FirstOrDefaultAsync(user => user.Login == requestDto.Login);
        if (user is null)
        {
            throw new NotFoundException($"Пользователь с Login: {requestDto.Login} не найден.");
        }

        if (!passwordHasherService.VerifyPassword(user.PasswordHash, requestDto.Password))
        {
            throw new BadRequestException("Неправильный пароль!");
        }

        var (token, refreshToken) = await jwtService.GetNewAccessTokenWithRefreshAsync(user);
        return new AuthResponse(token, refreshToken);
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest requestDto)
    {
        (string token, string refreshToken) =
            await jwtService.GetRefreshTokenAsync(requestDto.Token, requestDto.RefreshToken);
        return new RefreshTokenResponse(token, refreshToken);
    }
}