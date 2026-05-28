using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Exam.App.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ProjectDto> CreateAsync(ProjectDto dto, string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            StartedAt = dto.StartedAt,
            Status = ProjectStatus.Draft,
            UserId = user.Id
        };

        var created = await _projectRepository.CreateAsync(project);
        return _mapper.Map<ProjectDto>(created);
    }

    public async Task<ProjectDto> UpdateAsync(int id, ProjectDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
            throw new NotFoundException(id);

        project.Name = dto.Name;
        project.Description = dto.Description;

        await _projectRepository.UpdateAsync(project);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task DeleteAsync(int id)
    {
        await _projectRepository.DeleteAsync(id);
    }

    public async Task<List<ProjectDto>> GetByUserIdAsync(string userId)
    {
        var projects = await _projectRepository.GetVisibleByUserIdAsync(userId);
        return _mapper.Map<List<ProjectDto>>(projects);
    }

    public async Task<List<ProjectDto>> GetOwnedAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return await GetByUserIdAsync(user.Id);
    }
}
