using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.Task.Request;
using Services.Models.Task.Response;

namespace API.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketController(ITicketService service) : ControllerBase
{
    /// <summary>
    /// Получение всех задач
    /// </summary>
    /// <param name="requestDto">Объект пагинации</param>
    /// <returns>Request-модель списка задач</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetAllResponse<TicketResponse>), 200)]
    public async Task<IActionResult> GetAll([FromBody] GetAllRequest requestDto) => Ok(await service.GetAllAsync(requestDto));

    /// <summary>
    /// Получение задачи
    /// </summary>
    /// <param name="id">Guid задачи</param>
    /// <returns>Response-модель задачи</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TicketResponse), 200)]
    public async Task<IActionResult> Get(Guid id) => Ok(await service.GetByIdAsync(id));

    /// <summary>
    /// Создание задачи
    /// </summary>
    /// <param name="requestDto">Request-модель задачи</param>
    /// <returns>Response-модель задачи</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TicketResponse), 201)]
    public async Task<IActionResult> Post([FromBody] CreateTicketRequest requestDto)
    {
        var result = await service.CreateAsync(requestDto);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновление существующей задачи
    /// </summary>
    /// <param name="id">Guid задачи</param>
    /// <param name="requestDto">Request-модель задачи</param>
    /// <returns>Response-модель задачи</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TicketResponse), 200)]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateTicketRequest requestDto) => Ok(await service.UpdateAsync(id, requestDto));

    /// <summary>
    /// Удаление задачи
    /// </summary>
    /// <param name="id">Guid задачи</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);

        return NoContent();
    }

    [HttpPatch("{id:guid}/parameter")]
    [ProducesResponseType(typeof(TicketResponse), 200)]
    public async Task<IActionResult> Patch(Guid id, [FromBody] PatchTicketRequest requestDto)
    {
        return Ok(await service.SetDeadlineOrAssigneAsync(id, requestDto));
    }
}