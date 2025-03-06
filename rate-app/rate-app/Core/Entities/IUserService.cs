using rate_it_api.Core.Entities;

namespace rate_it_api.Core.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);

    }
}
