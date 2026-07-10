using Railway.Platform.Application.Interfaces;

namespace Railway.Platform.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public PasswordService() { }

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
