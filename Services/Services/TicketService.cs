using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.Tag.Response;
using Services.Models.Task.Request;
using Services.Models.Task.Response;

namespace Services.Services;

public class TicketService(ITicketRepository repository, ILogger<TicketService> logger) : ITicketService
{
    public async Task<GetAllResponse<TicketResponse>> GetAllAsync(GetAllRequest requestDto)
    {
        var query = repository.GetAll()
            .AsNoTracking()
            .Select(ticket => new TicketResponse(ticket.Id, ticket.Title, ticket.Description,  ticket.Status, ticket.Deadline, ticket.AssignedUserId, ticket.ProjectId, ticket.TicketTags.Select(tag => new TagResponse(tag.Tag.Id, tag.Tag.Name))));
        
        var count = query.Count();
        
        query = query.Skip((requestDto.PageNumber - 1) * requestDto.PageSize).Take(requestDto.PageSize).Take(requestDto.PageSize);

        return new GetAllResponse<TicketResponse>()
        {
            Data = await query.ToListAsync(),
            Count = count,
            PageNumber = requestDto.PageNumber,
            PageSize = requestDto.PageSize
        };
    }

    public async Task<TicketResponse> GetByIdAsync(Guid id)
    {
        try
        {
            var ticket = await repository.GetByIdAsync(id);

            if (ticket is null)
            {
                throw new NotFoundException("Ticket now found");
            }
            
            return new TicketResponse(ticket.Id, ticket.Title, ticket.Description, ticket.Status, ticket.Deadline,
                ticket.AssignedUserId, ticket.ProjectId,
                ticket.TicketTags.Select(tag => new TagResponse(tag.Tag.Id, tag.Tag.Name)));
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при получении задачи \n\t{}", exception.Message);
            throw;
        }
    }

    public async Task<TicketResponse> CreateAsync(CreateTicketRequest requestDto)
    {
        try
        {
            var ticket = await repository.CreateAsync(new Ticket
            {
                Title = requestDto.Title,
                Description = requestDto.Description,
                Deadline = requestDto.Deadline,
                ProjectId = requestDto.ProjectId,
                AssignedUserId = requestDto.AssignedUserId
            });
            
            return new TicketResponse(ticket.Entity.Id, ticket.Entity.Title, ticket.Entity.Description, ticket.Entity.Status, ticket.Entity.Deadline,
                ticket.Entity.AssignedUserId, ticket.Entity.ProjectId,
                ticket.Entity.TicketTags.Select(tag => new TagResponse(tag.Tag.Id, tag.Tag.Name)));
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при создании задачи \n\t{}", exception.Message);
            throw;
        }
    }

    public async Task<TicketResponse> UpdateAsync(Guid id, UpdateTicketRequest requestDto)
    {
        try
        {
            var ticket = await repository.GetByIdAsync(id);

            if (ticket is null)
            {
                throw new NotFoundException("Ticket now found");
            }
            
            ticket.Title = requestDto.Title;
            ticket.Description = requestDto.Description;
            ticket.Deadline = requestDto.Deadline;
            ticket.AssignedUserId = requestDto.AssignedUserId;
            ticket.ProjectId = requestDto.ProjectId;
            
            return new TicketResponse(ticket.Id, ticket.Title, ticket.Description, ticket.Status, ticket.Deadline,
                ticket.AssignedUserId, ticket.ProjectId,
                ticket.TicketTags.Select(tag => new TagResponse(tag.Tag.Id, tag.Tag.Name)));
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при обновлении задачи \n\t{}", exception.Message);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var ticket = await repository.GetByIdAsync(id);

            if (ticket is null)
            {
                throw new NotFoundException("Ticket now found");
            }

            await repository.DeleteAsync(ticket);
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при удалении задачи \n\t{}", exception.Message);
            throw;
        }
    }
}