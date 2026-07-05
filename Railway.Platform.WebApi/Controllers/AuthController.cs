using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Railway.Platform.WebApi.Entities;
using Railway.Platform.WebApi.Models;

namespace Railway.Platform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            var user = new User()
            {
                Username = request.UserName
            };

            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            return Ok(user);
        }

    }
}
