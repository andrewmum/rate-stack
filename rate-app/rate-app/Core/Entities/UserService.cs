using FirebaseAdmin.Auth;
using rate_it_api.Core.DTOs;
using rate_it_api.Core.Entities;
using rate_it_api.Infra.Repositories;

namespace rate_it_api.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<string> VerifyUserAsync(VerifyTokenDto request)
        {

            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.Token);

            string uid = decodedToken.Uid;
            string? email = decodedToken.Claims.GetValueOrDefault("email")?.ToString();
            string? name = decodedToken.Claims.GetValueOrDefault("name")?.ToString();

            if (string.IsNullOrEmpty("email")) return "Email is required";

            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null)
            {
                user = new User
                {
                    Id = uid,
                    Email = email,
                    Name = name ?? email.Split("@")[0],
                    CreatedAt = DateTime.UtcNow
                };
                await _userRepository.CreateUserAsync(user);

            }
            return user.Id;

        }



    }
}
