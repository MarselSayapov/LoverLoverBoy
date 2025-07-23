using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using Services.Interfaces;
using Services.Models.Auth.Requests;
using Services.Services;

namespace LoverLoverBoy.UnitTests;

public class AuthServiceUnitTests
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IJwtService> _jwtService;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
    private readonly Mock<ILogger<AuthService>> _logger;
    private readonly Mock<IPasswordHasherService> _passwordHasher;
    private readonly IAuthService _authService;

    public AuthServiceUnitTests()
    {
        _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        _userRepository = new Mock<IUserRepository>();
        _jwtService = new Mock<IJwtService>();
        _logger = new Mock<ILogger<AuthService>>();
        _passwordHasher = new Mock<IPasswordHasherService>();
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(uow => uow.Users)
            .Returns(_userRepository.Object);
        unitOfWork
            .Setup(uow => uow.RefreshTokens)
            .Returns(_refreshTokenRepository.Object);

        _authService = new AuthService(unitOfWork.Object, _jwtService.Object, _passwordHasher.Object, _logger.Object);
    }

    [Fact]
    public async Task Register_Should_ThrowConflictException_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = new RegisterRequest("johnDoe", "johnDoe@example.com", "admin");

        var users = new List<User>();
        for (var i = 0; i < 30; i++)
        {
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "johnDoe@example.com",
                Login = "johnDoe",
                PasswordHash = "afkmpafkmerkmferkmferkmnferknmf123213afjnjnk"
            });
        }

        var mock = users.AsQueryable()
            .BuildMock();
        _userRepository
            .Setup(repo => repo.GetAll())
            .Returns(mock);

        // Assert
        await Assert.ThrowsAsync<DuplicateException>(() => _authService.RegisterAsync(request));
    }

    [Fact]
    public async Task Register_Should_Succeed()
    {
        // Arrange
        var request = new RegisterRequest("johnDoe", "johnDoe@example.com", "admin");

        var users = new List<User>();
        for (var i = 0; i < 30; i++)
        {
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "zheny2004@example.com",
                Login = "zheny2004",
                PasswordHash = "afkmpafkmerkmferkmferkmnferknmf123213afjnjnk"
            });
        }

        _userRepository
            .Setup(repo => repo.GetAll())
            .Returns(users.AsQueryable());

        _passwordHasher
            .Setup(repo => repo.HashPassword("password"))
            .Returns("hashedPassword");

        _userRepository
            .Setup(repo => repo.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(new User());


        _jwtService
            .Setup(js => js.GetNewAccessTokenWithRefreshAsync(It.IsAny<User>()))
            .ReturnsAsync(("access_token", "refresh_token"));

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("access_token", result.Token);
        Assert.Equal("refresh_token", result.RefreshToken);
    }

    [Fact]
    public async Task Login_Should_Succeed()
    {
        // Arrange
        var request = new LoginRequest("johnDoe", "password");

        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Login = "johnDoe",
                PasswordHash = "hashedPassword",
                Email = "johnDoe@example.com"
            }
        };

        var mock = users.AsQueryable().BuildMock();

        _userRepository
            .Setup(repo => repo.GetAll())
            .Returns(mock);

        _passwordHasher
            .Setup(hasher => hasher.VerifyPassword("hashedPassword", "password"))
            .Returns(true);

        _jwtService
            .Setup(js => js.GetNewAccessTokenWithRefreshAsync(It.IsAny<User>()))
            .ReturnsAsync(("access_token", "refresh_token"));

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("access_token", result.Token);
        Assert.Equal("refresh_token", result.RefreshToken);
    }

    [Fact]
    public async Task Login_WithIncorrectPassword_Should_ThrowBadRequestException()
    {
        // Arrange
        var request = new LoginRequest("johnDoe", "password");

        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Login = "johnDoe",
                PasswordHash = "hashedP@ssword",
                Email = "johnDoe@example.com"
            }
        };

        var mock = users.AsQueryable().BuildMock();

        _userRepository
            .Setup(repo => repo.GetAll())
            .Returns(mock);

        _passwordHasher
            .Setup(hasher => hasher.VerifyPassword("hashedPassword", "password"))
            .Returns(true);

        _jwtService
            .Setup(js => js.GetNewAccessTokenWithRefreshAsync(It.IsAny<User>()))
            .ReturnsAsync(("access_token", "refresh_token"));

        // Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _authService.LoginAsync(request));
    }
}