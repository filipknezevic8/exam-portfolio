namespace Exam.App.Domain.Repositories;

public interface IUserRepository
{
    Task<PaginatedList<ApplicationUser>> GetAllAsync(int page, int pageSize);
}
