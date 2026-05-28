using Exam.App.Services;
using Exam.App.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationDto data)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _authService.Register(data);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto data)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _authService.Login(data);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        return Ok(await _authService.GetProfile(User));
    }
}