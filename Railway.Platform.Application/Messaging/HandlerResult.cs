using Railway.EventConsumer.Domain.Enums;

namespace Railway.EventConsumer.Application.Messaging
{
    public class HandlerResult
    {
        public HandlerResultStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
