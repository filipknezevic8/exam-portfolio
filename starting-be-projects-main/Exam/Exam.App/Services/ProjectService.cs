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
        if (user == null)
        {
            throw new UnauthorizedException("Korisnik nije prijavljen.");
        }

        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            StartedAt = dto.StartedAt,
            Status = ProjectStatus.Draft,
            CompletedAt = null,
            UserId = user.Id
        };

        var created = await _projectRepository.CreateAsync(project);
        return _mapper.Map<ProjectDto>(created);
    }

    public async Task<ProjectDto> UpdateAsync(int id, ProjectDto dto, string username)
    {
        var currentUser = await _userManager.FindByNameAsync(username);
        if (currentUser == null)
        {
            throw new UnauthorizedException("Korisnik nije prijavljen.");
        }

        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            throw new NotFoundException(id);
        }

        if (project.UserId != currentUser.Id)
        {
            throw new UnauthorizedException("Nemate pravo da menjate ovaj projekat.");
        }

        if (project.Status == ProjectStatus.Completed)
        {
            if (project.Name != dto.Name || project.Description != dto.Description || project.StartedAt != dto.StartedAt)
            {
                throw new BadRequestException("Kompletiran projekat nije moguće menjati.");
            }

            if (dto.Status.HasValue && dto.Status.Value == ProjectStatus.Draft)
            {
                project.Status = ProjectStatus.Draft;
                project.CompletedAt = null;
            }
        }
        else
        {
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.StartedAt = dto.StartedAt;

            if (dto.Status.HasValue)
            {
                project.Status = dto.Status.Value;
                if (project.Status == ProjectStatus.Draft)
                {
                    project.CompletedAt = null;
                }
            }
        }

        await _projectRepository.UpdateAsync(project);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> StartAsync(int id, string username)
    {
        var project = await GetOwnedProjectAsync(id, username);
        if (project.Status != ProjectStatus.Draft)
        {
            throw new BadRequestException("Samo projekat u pripremi može da se započne.");
        }

        project.Status = ProjectStatus.Published;
        await _projectRepository.UpdateAsync(project);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> ConcludeAsync(int id, string username)
    {
        var project = await GetOwnedProjectAsync(id, username);
        if (project.Status != ProjectStatus.Published)
        {
            throw new BadRequestException("Samo projekat u realizaciji može da se zaključi.");
        }

        project.Status = ProjectStatus.Completed;
        project.CompletedAt = DateTime.UtcNow;
        await _projectRepository.UpdateAsync(project);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> ReopenAsync(int id, string username)
    {
        var project = await GetOwnedProjectAsync(id, username);
        if (project.Status != ProjectStatus.Completed)
        {
            throw new BadRequestException("Samo kompletiran projekat može da se vrati u pripremu.");
        }

        project.Status = ProjectStatus.Draft;
        project.CompletedAt = null;
        await _projectRepository.UpdateAsync(project);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task DeleteAsync(int id, string username)
    {
        var project = await GetOwnedProjectAsync(id, username);
        await _projectRepository.DeleteAsync(project.Id);
    }

    public async Task<List<ProjectDto>> GetByUserIdAsync(string userId)
    {
        var projects = await _projectRepository.GetVisibleByUserIdAsync(userId);
        return _mapper.Map<List<ProjectDto>>(projects);
    }

    public async Task<List<ProjectDto>> GetOwnedAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new UnauthorizedException("Korisnik nije prijavljen.");
        }

        var projects = await _projectRepository.GetByUserIdAsync(user.Id);
        return _mapper.Map<List<ProjectDto>>(projects);
    }

    private async Task<Project> GetOwnedProjectAsync(int id, string username)
    {
        var currentUser = await _userManager.FindByNameAsync(username);
        if (currentUser == null)
        {
            throw new UnauthorizedException("Korisnik nije prijavljen.");
        }

        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            throw new NotFoundException(id);
        }

        if (project.UserId != currentUser.Id)
        {
            throw new UnauthorizedException("Nemate pravo da pristupite ovom projektu.");
        }

        return project;
    }
}
