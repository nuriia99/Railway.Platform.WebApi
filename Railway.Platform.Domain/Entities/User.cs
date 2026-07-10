namespace Railway.Platform.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        private User() { }

        public User(string userName, string password)
        {
            Id = Guid.NewGuid();
            Username = userName;
            PasswordHash = password;
        }
    }
}
