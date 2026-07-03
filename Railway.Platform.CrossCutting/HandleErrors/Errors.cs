using System.Net;

namespace Railway.EventConsumer.CrossCutting.HandleErrors
{
    public static class Errors
    {
        public static readonly Error MailNotFoundError = new(
            HttpStatusCode.NotFound, "Not found a user with that email.");
        public static readonly Error InvalidMessage = new(
            HttpStatusCode.BadRequest, "Message is null or not valid.");
    }
}
