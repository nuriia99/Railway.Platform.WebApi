namespace Railway.Platform.WebApi.Models
{
    public record RegisterUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
