using Exam.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "User")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _userService.GetAllUsersAsync(page, pageSize);
        return Ok(result);
    }
}
