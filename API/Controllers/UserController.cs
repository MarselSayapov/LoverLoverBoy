using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.GetAll.Requests;
using Services.Models.GetAll.Responses;
using Services.Models.User.Requests;
using Services.Models.User.Responses;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UserController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Получение всех пользователей
    /// </summary>
    /// <param name="requestDto">Объект пагинации</param>
    /// <returns>Response-модели пользователей, их количество и параметры пагинации</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetAllResponse<UserResponse>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllRequest requestDto) =>
        Ok(await userService.GetAllAsync(requestDto));

    /// <summary>
    /// Получение пользователя по Id
    /// </summary>
    /// <param name="id">Guid-идентификатор пользователя</param>
    /// <returns>Response-модель пользователя</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(Guid id) => Ok(await userService.GetByIdAsync(id));

    /// <summary>
    /// Обновление пользователя
    /// </summary>
    /// <param name="id">Guid-идентификатор пользователя</param>
    /// <param name="requestDto">Request-модель пользователя</param>
    /// <returns>Response-модель пользователя</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserRequest requestDto) => Ok(await userService.UpdateAsync(requestDto));

    /// <summary>
    /// Удаление пользователя
    /// </summary>
    /// <param name="id">Guid-идентификатор пользователя</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await userService.DeleteAsync(id);
        return NoContent();
    }
}