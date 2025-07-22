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
    private Mock<ILogger<AuthService>> _logger;
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
        
        unitOfWork.Setup(uow => uow.Users).Returns(_userRepository.Object);
        unitOfWork.Setup(uow => uow.RefreshTokens).Returns(_refreshTokenRepository.Object);
        
        _authService = new AuthService(unitOfWork.Object, _jwtService.Object, _passwordHasher.Object, _logger.Object);
    }

    [Fact]
    public async Task Register_Should_ThrowConflictException()
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

        var mock = users.AsQueryable().BuildMock();
        _userRepository.Setup(repo => repo.GetAll()).Returns(mock);
        
        // Assert
        await Assert.ThrowsAsync<DuplicateException>(() => _authService.RegisterAsync(request));
    }
}