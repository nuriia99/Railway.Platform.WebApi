namespace Railway.Platform.WebApi.Models
{
    public record LoginUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
