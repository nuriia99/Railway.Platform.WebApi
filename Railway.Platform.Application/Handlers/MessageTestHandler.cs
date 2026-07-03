using Railway.EventConsumer.Application.Messaging;
using Railway.EventConsumer.Domain.Enums;
using Railway.EventConsumer.Domain.Events;
using Railway.EventConsumer.Domain.Exceptions;
using System.Text.Json;

namespace Railway.EventConsumer.Application.Handlers
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
