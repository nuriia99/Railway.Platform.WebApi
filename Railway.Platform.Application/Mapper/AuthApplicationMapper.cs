using Railway.Platform.Domain.Entities;
using Railway.Platform.WebApi.Models;

namespace Railway.Platform.Application.Mappers
{
    public static class AuthApplicationMapper
    {
        public static User MapToUser(RegisterUserDto request, string passwordHash)
        => new(request.UserName, passwordHash);
    }
}
