using Railway.Platform.CrossCutting.HandleErrors;

namespace Railway.Platform.Application.Messaging
{
    public interface IMessageDispatcher
    {
        Task<HandlerResult> DispatchAsync(string json, int retryCount);
    }
}