using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Railway.Platform.Application.Interfaces;
using Railway.Platform.Domain.Entities;
using Railway.Platform.WebApi.Mappers;
using Railway.Platform.WebApi.Models;

namespace Railway.Platform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUserRequest request)
        {
            var result = await _authService.RegisterAsync(AuthWebApiMapper.MapToRegisterUserDto(request));
            if (result.HasErrors())
            {
                var error = result.Errors!.First();
                return StatusCode((int)error.Code, error);
            }

            return Ok(AuthWebApiMapper.MapToRegisterUserResponse(result.Result!));
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginUserRequest request)
        {
            var result = await _authService.LoginAsync(AuthWebApiMapper.MapToLoginUserDto(request));
            if (result.HasErrors())
            {
                var error = result.Errors!.First();
                return StatusCode((int)error.Code, error);
            }

            return Ok(AuthWebApiMapper.MapToLoginUserResponse(result.Result!));
        }
    }
}
