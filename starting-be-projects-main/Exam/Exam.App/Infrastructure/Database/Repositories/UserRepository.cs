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
        var totalCount = await _context.Users.CountAsync();

        var items = await _context.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<ApplicationUser>(items, page, pageSize, totalCount);
    }
}
