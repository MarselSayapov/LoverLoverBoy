using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.Project.Request;
using Services.Models.Project.Response;
using Services.Models.Task.Request;

namespace Services.Interfaces;

public interface IProjectService
{
    Task<GetAllResponse<ProjectResponse>> GetAllAsync(GetAllRequest requestDto);
    Task<ProjectResponse> GetByIdAsync(Guid id);
    Task<ProjectResponse> CreateAsync(CreateProjectRequest requestDto);
    Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest requestDto);
    Task DeleteAsync(Guid id);
}