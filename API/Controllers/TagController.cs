using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.Tag.Request;
using Services.Models.Tag.Response;

namespace API.Controllers;

[ApiController]
[Route("api/tags")]
public class TagController(ITagService service) : ControllerBase
{
    /// <summary>
    /// Получение всех тегов
    /// </summary>
    /// <param name="requestDto">Объект пагинации</param>
    /// <returns>Response-модель списка всех проектов</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetAllResponse<TagResponse>), 200)]
    public async Task<IActionResult> GetAll([FromBody] GetAllRequest requestDto) => Ok(await service.GetAllAsync(requestDto));

    /// <summary>
    /// Получение тега по Guid
    /// </summary>
    /// <param name="id">Guid тега</param>
    /// <returns>Response-модель тега</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TagResponse), 200)]
    public async Task<IActionResult> Get(Guid id) => Ok(await service.GetByIdAsync(id));

    /// <summary>
    /// Создание тега
    /// </summary>
    /// <param name="requestDto">Request-модель тега</param>
    /// <returns>Response-модель тега</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TagResponse), 201)]
    public async Task<IActionResult> Post([FromBody] CreateTagRequest requestDto)
    {
        var result = await service.CreateAsync(requestDto);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
    
    /// <summary>
    /// Обновление существующего тега
    /// </summary>
    /// <param name="id">Guid тега</param>
    /// <param name="requestDto">Request-модель тега</param>
    /// <returns>Response-модель тега</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TagResponse), 200)]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateTagRequest requestDto) => Ok(await service.UpdateAsync(id, requestDto));
    
    /// <summary>
    /// Удаление тега
    /// </summary>
    /// <param name="id">Guid тега</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);

        return NoContent();
    }
}