using Railway.EventConsumer.CrossCutting.HandleErrors;
using Railway.EventConsumer.Domain.Enums;
using System.Text.Json;

namespace Railway.EventConsumer.Application.Messaging
{
    public interface IMessageHandler
    {
        string MessageType { get; }
        Task<HandlerResult> HandleAsync(JsonElement data, int retryCount);
    }
}
