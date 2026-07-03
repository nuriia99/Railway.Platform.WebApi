using Railway.Platform.CrossCutting.HandleErrors;
using Railway.Platform.Domain.Enums;
using System.Text.Json;

namespace Railway.Platform.Application.Messaging
{
    public interface IMessageHandler
    {
        string MessageType { get; }
        Task<HandlerResult> HandleAsync(JsonElement data, int retryCount);
    }
}
