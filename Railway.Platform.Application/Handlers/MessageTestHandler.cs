using Railway.Platform.Application.Messaging;
using Railway.Platform.Domain.Enums;
using Railway.Platform.Domain.Events;
using Railway.Platform.Domain.Exceptions;
using System.Text.Json;

namespace Railway.Platform.Application.Handlers
{
    public class MessageTestHandler : IMessageHandler
    {
        public string MessageType => "MessageTest";

        public Task<HandlerResult> HandleAsync(JsonElement data, int retryCount)
        {
            var message = JsonSerializer.Deserialize<MessageTest>(data.GetRawText());
            if (message != null)
            {
                Console.WriteLine($"Procesando mensaje {message.MessageId}");

                if (message.Amount < 0)
                {
                    throw new TestException("Test retry policy");
                }
            }
            return Task.FromResult(new HandlerResult { Status = HandlerResultStatus.Success });
        }
    }
}
