namespace Railway.Platform.Domain.Events
{
    public record EmailMessaje
    {
        public required string To { get; set; } 
        public required string Subject { get; set; }
        public required string Body { get; set; }
    }
}
