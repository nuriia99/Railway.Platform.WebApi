
using Railway.Platform.CrossCutting.HandleErrors;
using Railway.Platform.Domain.Entities;
using Railway.Platform.WebApi.Models;

namespace Railway.Platform.Application.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult<User?>> RegisterAsync(RegisterUserDto request);
    }
}