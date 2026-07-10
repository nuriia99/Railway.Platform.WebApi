using Railway.Platform.Domain.Interfaces;
using Railway.Platform.Domain.Entities;
using Railway.Platform.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Railway.Platform.CrossCutting.HandleErrors;

namespace Railway.Platform.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public UserRepository(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<OperationResult<User?>> RegisterAsync(User request)
        {
            if(await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return new OperationResult<User?>().AddError(Errors.InvalidUser);
            }

            _context.Users.Add(request);
            await _context.SaveChangesAsync();

            return new OperationResult<User?>().AddResult(request);
        }
    }
}
