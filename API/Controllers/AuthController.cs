using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Auth.Requests;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest requestDto)
    {
        var result = await authService.RegisterAsync(requestDto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest requestDto)
    {
        var result = await authService.LoginAsync(requestDto);
        return Ok(result);
    }
}