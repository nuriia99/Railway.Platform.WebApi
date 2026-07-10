using Railway.Platform.Application.Interfaces;
using Railway.Platform.CrossCutting.HandleErrors;
using Railway.Platform.Domain.Entities;
using Railway.Platform.Domain.Interfaces;
using Railway.Platform.Application.Mappers;
using Railway.Platform.WebApi.Models;

namespace Railway.Platform.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        public AuthService(IUserRepository authRepository, IPasswordService passwordService)
        {
            _userRepository = authRepository;
            _passwordService = passwordService;
        }

        public async Task<OperationResult<User?>> RegisterAsync(RegisterUserDto request)
        {
            var user = AuthApplicationMapper.MapToUser(
                request,
                _passwordService.Hash(request.Password)
            );

            var result = await _userRepository.RegisterAsync(user);
            if(result.HasErrors())
                return new OperationResult<User?>().AddErrors(result.Errors!);

            return new OperationResult<User?>().AddResult(result.Result);
        }
    }
}
