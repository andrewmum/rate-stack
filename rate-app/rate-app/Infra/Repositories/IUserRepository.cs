using rate_it_api.Core.Entities;

namespace rate_it_api.Infra.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
