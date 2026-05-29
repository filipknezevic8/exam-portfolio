using System.Security.Claims;
using Exam.App.Services;
using Exam.App.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers;

[Route("api/projects")]
[ApiController]
[Authorize(Roles = "User")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectDto dto)
    {
        var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _projectService.CreateAsync(dto, username);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectDto dto)
    {
        var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _projectService.UpdateAsync(id, dto, username);
        return Ok(result);
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> Start(int id)
    {
        var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _projectService.StartAsync(id, username);
        return Ok(result);
    }

    [HttpPost("{id}/conclude")]
    public async Task<IActionResult> Conclude(int id)
    {
        var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _projectService.ConcludeAsync(id, username);
        return Ok(result);
    }

    [HttpPost("{id}/reopen")]
    public async Task<IActionResult> Reopen(int id)
    {
        var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _projectService.ReopenAsync(id, username);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _projectService.DeleteAsync(id, username);
        return Ok();
    }

    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetByUser(string userId)
    {
        var result = await _projectService.GetByUserIdAsync(userId);
        return Ok(result);
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _projectService.GetOwnedAsync(username);
        return Ok(result);
    }
}
