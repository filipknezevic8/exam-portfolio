using Exam.App.Services.Dtos;

namespace Exam.App.Services;

public interface IUserService
{
    Task<PaginatedListDto<ProfileDto>> GetAllUsersAsync(int page, int pageSize);
}
