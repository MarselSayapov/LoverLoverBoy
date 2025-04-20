using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.User.Requests;
using Services.Models.User.Responses;

namespace Services.Interfaces;

public interface IUserService
{
    public Task<GetAllResponse<UserResponse>> GetAllAsync(GetAllRequest requestDto);
    public Task<UserResponse> GetByIdAsync(Guid id);
    public Task<UserResponse> UpdateAsync(UserRequest requestDto);
    public Task DeleteAsync(Guid id);
}