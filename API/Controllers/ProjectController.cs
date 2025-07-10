using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.Project.Request;
using Services.Models.Project.Response;

namespace API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController(IProjectService service) : ControllerBase
{
    /// <summary>
    /// Получение всех проектов
    /// </summary>
    /// <param name="requestDto">Объект пагинации</param>
    /// <returns>Response-модель списка проектов</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetAllResponse<ProjectResponse>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllRequest requestDto)
    {
        return Ok(await service.GetAllAsync(requestDto));
    }

    /// <summary>
    /// Получение проекта по Guid
    /// </summary>
    /// <param name="id">Guid проекта</param>
    /// <returns>Response-модель проекта</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetAllResponse<ProjectResponse>), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await service.GetByIdAsync(id));
    }

    /// <summary>
    /// Создание нового проекта
    /// </summary>
    /// <param name="requestDto">Request-модель проекта</param>
    /// <returns>Response-модель проекта</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GetAllResponse<ProjectResponse>), 201)]
    public async Task<IActionResult> Post([FromBody] CreateProjectRequest requestDto)
    {
        var result = await service.CreateAsync(requestDto);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновление существующего проекта
    /// </summary>
    /// <param name="id">Guid проекта</param>
    /// <param name="requestDto">Request-модель проекта</param>
    /// <returns>Response-модель проекта</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GetAllResponse<ProjectResponse>), 200)]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProjectRequest requestDto)
    {
        return Ok(await service.UpdateAsync(id, requestDto));
    }

    /// <summary>
    /// Удаление проекта
    /// </summary>
    /// <param name="id">Guid проекта</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);

        return NoContent();
    }
}