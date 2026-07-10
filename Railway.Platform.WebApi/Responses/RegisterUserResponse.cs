namespace Railway.Platform.WebApi.Models
{
    public record RegisterUserResponse
    {
        public Guid Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public RegisterUserResponse(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}
