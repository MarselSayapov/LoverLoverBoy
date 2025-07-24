using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Auth.Requests;
using Services.Models.Auth.Responses;

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
    [ProducesResponseType(typeof(AuthResponse), 200)]
    public async Task<IActionResult> Register(RegisterRequest requestDto) => Ok(await authService.RegisterAsync(requestDto));

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="requestDto">Request-объект</param>
    /// <returns>JWT token и Refresh Token</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Login([FromBody] LoginRequest requestDto) => Ok(await authService.LoginAsync(requestDto));

    /// <summary>
    /// Обновление токена
    /// </summary>
    /// <param name="requestDto">Request-объект</param>
    /// <returns>Обновленный JWT Token и новый Refresh Token</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponse), 200)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest requestDto) => Ok(await authService.RefreshTokenAsync(requestDto));

}