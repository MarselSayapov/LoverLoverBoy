using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.Task.Request;
using Services.Models.Task.Response;

namespace Services.Interfaces;

public interface ITicketService
{
    Task<GetAllResponse<TicketResponse>> GetAllAsync(GetAllRequest requestDto);
    Task<TicketResponse> GetByIdAsync(Guid id);
    Task<TicketResponse> CreateAsync(CreateTicketRequest requestDto);
    Task<TicketResponse> UpdateAsync(Guid id, UpdateTicketRequest requestDto);
    Task DeleteAsync(Guid id);
}