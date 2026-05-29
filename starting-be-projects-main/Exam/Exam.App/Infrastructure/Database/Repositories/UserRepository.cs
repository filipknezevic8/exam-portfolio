using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<UserSummaryDto>> GetAllAsync(int page, int pageSize)
    {
        var query = from user in _context.Users
                    join userRole in _context.UserRoles on user.Id equals userRole.UserId
                    join role in _context.Roles on userRole.RoleId equals role.Id
                    where role.Name == "User"
                    select new UserSummaryDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                        CompletedProjectsCount = _context.Projects.Count(p => p.UserId == user.Id && p.Status == ProjectStatus.Completed),
                        InProgressProjectsCount = _context.Projects.Count(p => p.UserId == user.Id && p.Status == ProjectStatus.Published),
                        LastCompletedAt = _context.Projects
                            .Where(p => p.UserId == user.Id && p.Status == ProjectStatus.Completed)
                            .Select(p => p.CompletedAt)
                            .Max()
                    };

        query = query
            .Distinct()
            .OrderByDescending(u => u.CompletedProjectsCount)
            .ThenByDescending(u => u.InProgressProjectsCount)
            .ThenBy(u => u.Name)
            .ThenBy(u => u.Surname);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<UserSummaryDto>(items, page, pageSize, totalCount);
    }
}
