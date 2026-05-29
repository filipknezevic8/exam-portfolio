using Exam.App.Services.Dtos;

namespace Exam.App.Domain.Repositories;

public interface IUserRepository
{
    Task<PaginatedList<UserSummaryDto>> GetAllAsync(int page, int pageSize);
}
