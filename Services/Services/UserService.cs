using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.User.Requests;
using Services.Models.User.Responses;

namespace Services.Services;

public class UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger) : IUserService
{
    public async Task<GetAllResponse<UserResponse>> GetAllAsync(GetAllRequest requestDto)
    {
        var result = unitOfWork.Users.GetAll()
            .OrderBy(user => user.Id)
            .Select(user => new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Login = user.Login,
            });
        
        var count = result.Count();
        
        result = result.Skip((requestDto.PageNumber - 1) * requestDto.PageSize)
            .Take(requestDto.PageSize);
        return new GetAllResponse<UserResponse>
        {
            Data = await result.ToListAsync(),
            Count = count,
            PageSize = requestDto.PageSize,
            PageNumber = requestDto.PageNumber
        };
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        try
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundException($"Пользователь Guid: {id} не найден");
            }

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Login = user.Login
            };
        }
        catch (NotFoundException)
        {
            logger.LogError("Не найден пользователь Guid: {}", id);
            throw;
        }
        catch (Exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при поиске пользователя\n\tGuid: {}", id);
            throw;
        }
        
    }

    public async Task<UserResponse> UpdateAsync(UserRequest requestDto)
    {
        try
        {
            var user = await unitOfWork.Users.GetByEmailAsync(requestDto.Email);

            if (user == null)
            {
                throw new NotFoundException($"Пользователь с Email: {requestDto.Email} не найден");
            }

            if (!string.IsNullOrEmpty(requestDto.Login))
            {
                user.Login = requestDto.Login;
            }
        
            await unitOfWork.Users.UpdateAsync(user);

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Login = user.Login
            };
        }
        catch (Exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при обновлении пользователя\n\tEmail: {}",
                requestDto.Email);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"Пользователь с Guid: {id} не найден.");
            }
            
            await unitOfWork.Users.DeleteAsync(user);
        }
        catch (Exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при удалении пользователя\n\tGuid: {}", id);
            throw;
        }
    }
}