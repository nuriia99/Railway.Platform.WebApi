using Railway.Platform.Domain.Entities;
using Railway.Platform.WebApi.Models;

namespace Railway.Platform.WebApi.Mappers
{
    public static class AuthWebApiMapper
    {
        public static RegisterUserDto MapToRegisterUserDto(RegisterUserRequest request)
        => new(request.UserName, request.Password);
        public static RegisterUserResponse MapToRegisterUserResponse(User user)
        => new(user.Id, user.Username);
        public static LoginUserDto MapToLoginUserDto(LoginUserRequest request)
        => new(request.UserName, request.Password);
        public static LoginUserResponse MapToLoginUserResponse(User user)
        => new(user.Id, user.Username);
    }
}
