using Railway.Platform.Domain.Enums;

namespace Railway.Platform.Application.Messaging
{
    public class HandlerResult
    {
        public HandlerResultStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
