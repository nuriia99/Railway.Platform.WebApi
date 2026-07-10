namespace Railway.Platform.WebApi.Models
{
    public record LoginUserResponse
    {
        public Guid Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public LoginUserResponse(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}
