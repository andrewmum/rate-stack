using Microsoft.AspNetCore.Mvc;
using rate_it_api.Core.DTOs;
using rate_it_api.Core.Services;

namespace rate_it_api.Api.Controllers
{
    [Route("api/users")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("Verify")]
        public async Task<IActionResult> VerifyFirebaseToken([FromBody] VerifyTokenDto request)
        {
            var result = await _userService.VerifyUserAsync(request);
            if (result == null) return NotFound(result);
            return Ok(result);
        }
    }

} 
