﻿using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.Tag.Request;
using Services.Models.Tag.Response;

namespace Services.Services;

public class TagService(IUnitOfWork unitOfWork, ILogger<TagService> logger) : ITagService
{
    public async Task<GetAllResponse<TagResponse>> GetAllAsync(GetAllRequest requestDto)
    {
        try
        {
            var query = unitOfWork.Tags.GetAll()
                .Select(tag => new TagResponse(tag.Id, tag.Name));

            var count = query.Count();

            query = query.Skip((requestDto.PageNumber - 1) * requestDto.PageSize).Take(requestDto.PageSize);

            return new GetAllResponse<TagResponse>
            {
                Data = await query.ToListAsync(),
                Count = count,
                PageNumber = requestDto.PageNumber,
                PageSize = requestDto.PageSize
            };
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при получении тегов \n\t{}", exception.Message);
            throw;
        }
    }

    public async Task<TagResponse> GetByIdAsync(Guid id)
    {
        try
        {
            var tag = await unitOfWork.Tags.GetByIdAsync(id);

            if (tag is null)
            {
                throw new NotFoundException("Tag not found");
            }

            return new TagResponse(tag.Id, tag.Name);
        }
        catch (Exception exception)
        {
            logger.LogError("Произошла непредвиденная ошибка при получении тега \n\t{}", exception.Message);
            throw;
        }
    }

    public async Task<TagResponse> CreateAsync(CreateTagRequest requestDto)
    {
        var tag = await unitOfWork.Tags.CreateAsync(new Tag
        {
            Name = requestDto.Name
        });

        return new TagResponse(tag.Id, tag.Name);
    }

    public async Task<TagResponse> UpdateAsync(Guid id, UpdateTagRequest requestDto)
    {
        var tag = await unitOfWork.Tags.GetByIdAsync(id);

        if (tag is null)
        {
            throw new NotFoundException("Tag not found");
        }

        tag.Name = requestDto.Name;

        await unitOfWork.Tags.UpdateAsync(tag);

        return new TagResponse(tag.Id, tag.Name);
    }

    public async Task DeleteAsync(Guid id)
    {
        var tag = await unitOfWork.Tags.GetByIdAsync(id);

        if (tag is null)
        {
            throw new NotFoundException("Tag not found");
        }

        await unitOfWork.Tags.DeleteAsync(tag);
    }
}