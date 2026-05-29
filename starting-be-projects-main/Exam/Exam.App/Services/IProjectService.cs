using Exam.App.Services.Dtos;

namespace Exam.App.Services;

public interface IProjectService
{
    Task<ProjectDto> CreateAsync(ProjectDto dto, string username);
    Task<ProjectDto> UpdateAsync(int id, ProjectDto dto, string username);
    Task DeleteAsync(int id, string username);
    Task<List<ProjectDto>> GetByUserIdAsync(string userId);
    Task<List<ProjectDto>> GetOwnedAsync(string username);
}
