using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rate_it_api.Core.Entities;

namespace rate_app.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("verify-firebase-token")]
        public async Task<IActionResult> VerifyFirebaseToken([FromBody] VerifyTokenRequest request)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(request.Token);

                string uid = decodedToken.Uid;
                string? email = decodedToken.Claims.GetValueOrDefault("email")?.ToString();
                string? name = decodedToken.Claims.GetValueOrDefault("name")?.ToString();

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new { message = "Email is required" });
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    user = new User
                    {
                        Id = uid,
                        Email = email,
                        Name = name ?? email.Split('@')[0],
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { userId = user.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid token", error = ex.Message });
            }
        }
        public class VerifyTokenRequest
        {
            public string Token { get; set; } = string.Empty;
        }

    }
}
