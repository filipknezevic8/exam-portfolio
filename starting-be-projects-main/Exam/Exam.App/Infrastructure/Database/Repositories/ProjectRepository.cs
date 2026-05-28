using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Project> CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
    }

    public async Task<List<Project>> GetByUserIdAsync(string userId)
    {
        return await _context.Projects
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Project>> GetVisibleByUserIdAsync(string userId)
    {
        return await _context.Projects
            .Where(p =>
                p.UserId == userId &&
                (p.Status == ProjectStatus.Published || p.Status == ProjectStatus.Completed))
            .ToListAsync();
    }

    public async Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
