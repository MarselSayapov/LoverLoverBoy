using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.Tag.Request;
using Services.Models.Tag.Response;

namespace Services.Interfaces;

public interface ITagService
{
    Task<GetAllResponse<TagResponse>> GetAllAsync(GetAllRequest requestDto);
    Task<TagResponse> GetByIdAsync(Guid id);
    Task<TagResponse> CreateAsync(CreateTagRequest requestDto);
    Task<TagResponse> UpdateAsync(Guid id, UpdateTagRequest requestDto);
    Task DeleteAsync(Guid id);
}