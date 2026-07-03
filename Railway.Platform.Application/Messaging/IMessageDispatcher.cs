using Railway.EventConsumer.CrossCutting.HandleErrors;

namespace Railway.EventConsumer.Application.Messaging
{
    public interface IMessageDispatcher
    {
        Task<HandlerResult> DispatchAsync(string json, int retryCount);
    }
}