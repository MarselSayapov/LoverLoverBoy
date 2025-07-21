using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using Services.Models.GetAll.Requests;
using Services.Models.User.Requests;
using Services.Services;

namespace LoverLoverBoy.UnitTests;

public class UserServiceUnitTests
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly UserService _userService;

    public UserServiceUnitTests()
    {
        var logger = new Mock<ILogger<UserService>>();
        
        _userRepository = new Mock<IUserRepository>();
        
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(uow => uow.Users).Returns(_userRepository.Object);
        
        _userService = new UserService(unitOfWork.Object, logger.Object);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnPaginated()
    {
        // Arrange
        var request = new GetAllRequest
        {
            PageNumber = 1,
            PageSize = 15
        };


        var users = new List<User>();
        for (var i = 0; i < 30; i++)
        {
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "johndoe@example.com",
                Login = "johnDoe",
                PasswordHash = "afkmpafkmerkmferkmferkmnferknmf123213afjnjnk"
            });
        }
        
        var mock = users.AsQueryable().BuildMock();
        
        _userRepository.Setup(repo => repo.GetAll()).Returns(mock);
        // Act
        var result = await _userService.GetAllAsync(request);
        
        // Assert
        Assert.Equal(15, result.Data.Count());
    }

    [Fact]
    public async Task GetAllUsers_WhenPageSizeZero_ShouldReturnAllUsers()
    {
        // Arrange
        var request = new GetAllRequest
        {
            PageNumber = 0,
            PageSize = 0
        };
        
        var users = new List<User>();
        for (var i = 0; i < 30; i++)
        {
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "johndoe@example.com",
                Login = "johnDoe",
                PasswordHash = "afkmpafkmerkmferkmferkmnferknmf123213afjnjnk"
            });
        }
        
        var mock = users.AsQueryable().BuildMock();
        
        _userRepository.Setup(repo => repo.GetAll()).Returns(mock);
        
        // Act
        var result = await _userService.GetAllAsync(request);
        
        // Assert
        Assert.Equal(30, result.Data.Count());
    }

    [Fact]
    public async Task GetUserById_ShouldReturnCorrectUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "johndoe@example.com",
            Login = "johnDoe",
            PasswordHash = "afkmpafkmerkmferkmferkmnferknmf123213afjnjnk"
        };
        
        _userRepository.Setup(repo => repo.GetByIdAsync(user.Id)).ReturnsAsync(user);
        
        // Act

        var result = await _userService.GetByIdAsync(user.Id);
        
        // Assert
        
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task UpdateUser_WithIncorrectEmail_Should_ThrowNotFoundException()
    {
        // Arrange
        var request = new UserRequest
        {
            Email = "invalid-email@example.com",
            Login = "invalidLogin",
            Password = "1231231312312312312"
            
        };
        
        // Assert

        await Assert.ThrowsAsync<NotFoundException>(() => _userService.UpdateAsync(request));
    }
}
