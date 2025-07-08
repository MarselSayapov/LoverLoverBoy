using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Auth.Requests;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="requestDto">Request-объект</param>
    /// <returns>JWT token и Refresh Token</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest requestDto)
    {
        return Ok(await authService.RegisterAsync(requestDto));
    }
    
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="requestDto">Request-объект</param>
    /// <returns>JWT token и Refresh Token</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest requestDto)
    {
        return Ok(await authService.LoginAsync(requestDto));
    }

    /// <summary>
    /// Обновление токена
    /// </summary>
    /// <param name="requestDto">Request-объект</param>
    /// <returns>Обновленный JWT Token и новый Refresh Token</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest requestDto)
    {
        return Ok(await authService.RefreshTokenAsync(requestDto));
    }

}