using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.Project.Request;
using Services.Models.Project.Response;
using Services.Models.Tag.Response;
using Services.Models.Task.Response;

namespace Services.Services;

public class ProjectService(IUnitOfWork unitOfWork,  ILogger<ProjectService> logger) :  IProjectService
{
    public async Task<GetAllResponse<ProjectResponse>> GetAllAsync(GetAllRequest requestDto)
    {
        try
        {
            var query = unitOfWork.Projects.GetAll()
                .Select(project => new ProjectResponse(project.Id, project.Name, project.OwnerId,
                    project.Tickets.Select(ticket => new ProjectTicketResponse(ticket.Title, ticket.Description, ticket.Status, ticket.Deadline,
                        ticket.AssignedUserId,
                        ticket.TicketTags.Select(tag => new TagResponse(tag.Tag.Id, tag.Tag.Name))))));
            
            var count =  query.Count();
        
            query = query.Skip((requestDto.PageNumber - 1) * requestDto.PageSize).Take(requestDto.PageSize);

            return new GetAllResponse<ProjectResponse>
            {
                Data = await query.ToListAsync(),
                Count = count,
                PageNumber = requestDto.PageNumber,
            };
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при получении проектов \n\t{}", exception.Message);
            throw;
        }
    }

    public async Task<ProjectResponse> GetByIdAsync(Guid id)
    {
        try
        {
            var project = await unitOfWork.Projects.GetByIdAsync(id);

            if (project is null)
            {
                throw new NotFoundException("Project not found");
            }

            return new ProjectResponse(project.Id, project.Name, project.OwnerId, project.Tickets.Select(ticket =>
                new ProjectTicketResponse(ticket.Title, ticket.Description, ticket.Status, ticket.Deadline,
                    ticket.AssignedUserId,
                    ticket.TicketTags.Select(tag => new TagResponse(tag.Tag.Id, tag.Tag.Name)))));
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при получении проекта \n\t{}", exception.Message);
            throw;
        }
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest requestDto)
    {
        try
        {
            var project = await unitOfWork.Projects.CreateAsync(new Project
            {
                Name = requestDto.Name,
                OwnerId = requestDto.OwnerId
            });
            
            return new ProjectResponse(project.Entity.Id, project.Entity.Name, project.Entity.OwnerId, project.Entity.Tickets.Select(ticket =>
                new ProjectTicketResponse(ticket.Title, ticket.Description, ticket.Status, ticket.Deadline,
                    ticket.AssignedUserId,
                    ticket.TicketTags.Select(tag => new TagResponse(tag.Tag.Id, tag.Tag.Name)))));
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при создании проекта \n\t{}", exception.Message);
            throw;
        }
    }

    public async Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest requestDto)
    {
        try
        {
            var project = await unitOfWork.Projects.GetByIdAsync(id);

            if (project is null)
            {
                throw new NotFoundException("Project not found");
            }
            
            project.Name = requestDto.Name;
            project.OwnerId = requestDto.OwnerId;
            
            await unitOfWork.Projects.UpdateAsync(project);

            return new ProjectResponse(project.Id, project.Name, project.OwnerId, project.Tickets.Select(ticket =>
                new ProjectTicketResponse(ticket.Title, ticket.Description, ticket.Status, ticket.Deadline,
                    ticket.AssignedUserId,
                    ticket.TicketTags.Select(tag => new TagResponse(tag.Tag.Id, tag.Tag.Name)))));
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при обновлении проекта \n\t{}", exception.Message);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var project = await unitOfWork.Projects.GetByIdAsync(id);

            if (project is null)
            {
                throw new NotFoundException("Project not found");
            }
        
            await unitOfWork.Projects.DeleteAsync(project);
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при удалении проекта \n\t{}", exception.Message);
            throw;
        }
    }
}