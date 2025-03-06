namespace rate_it_api.Core.DTOs
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class VerifyTokenDto
    {
        public string Token { get; set; } = string.Empty;
    }
}
