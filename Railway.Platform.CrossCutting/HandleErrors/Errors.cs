using System.Net;

namespace Railway.Platform.CrossCutting.HandleErrors
{
    public static class Errors
    {
        public static readonly Error MailNotFoundError = new(
            HttpStatusCode.NotFound, "Not found a user with that email.");
        public static readonly Error InvalidMessage = new(
            HttpStatusCode.BadRequest, "Message is null or not valid.");
        public static readonly Error UserAlreadyExists = new(
            HttpStatusCode.BadRequest, "User already exists.");
        public static readonly Error InvalidCredentials = new(
            HttpStatusCode.Unauthorized, "Invalid username or password.");
    }
}
