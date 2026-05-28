namespace Exam.App.Domain.Repositories;

public interface IProjectRepository
{
    Task<Project> CreateAsync(Project project);
    Task<Project?> GetByIdAsync(int id);
    Task<List<Project>> GetByUserIdAsync(string userId);
    Task<List<Project>> GetVisibleByUserIdAsync(string userId);
    Task UpdateAsync(Project project);
    Task DeleteAsync(int id);
}
