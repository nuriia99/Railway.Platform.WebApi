using Railway.Platform.CrossCutting.HandleErrors;
using Railway.Platform.Domain.Entities;

namespace Railway.Platform.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult<User?>> RegisterAsync(User request);
        Task<User?> GetByUsernameAsync(string username);
    }
}