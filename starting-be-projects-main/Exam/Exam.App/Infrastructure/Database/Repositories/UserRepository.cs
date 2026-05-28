using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ApplicationUser>> GetAllAsync(int page, int pageSize)
    {
        var usersQuery =
            from user in _context.Users
            join userRole in _context.UserRoles on user.Id equals userRole.UserId
            join role in _context.Roles on userRole.RoleId equals role.Id
            where role.Name == "User"
            select user;

        usersQuery = usersQuery
            .Distinct()
            .OrderBy(u => u.Name)
            .ThenBy(u => u.Surname)
            .ThenBy(u => u.UserName);

        var totalCount = await usersQuery.CountAsync();

        var items = await usersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<ApplicationUser>(items, page, pageSize, totalCount);
    }
}
