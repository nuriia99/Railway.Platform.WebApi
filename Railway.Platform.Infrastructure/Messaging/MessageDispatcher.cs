using Railway.Platform.Application.Messaging;
using System.Text.Json;

namespace Railway.Platform.Infrastructure.Messaging
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly Dictionary<string, IMessageHandler> _handlers;

        public MessageDispatcher(IEnumerable<IMessageHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.MessageType);
        }

        public async Task<HandlerResult> DispatchAsync(string json, int retryCount)
        {
            var doc = JsonDocument.Parse(json);
            var type = doc.RootElement.GetProperty("type").GetString();
            var data = doc.RootElement.GetProperty("data");

            if (!_handlers.TryGetValue(type!, out var handler))
                throw new Exception($"No handler found for message type {type}");

            return await handler.HandleAsync(data, retryCount);
        }
    }

}
