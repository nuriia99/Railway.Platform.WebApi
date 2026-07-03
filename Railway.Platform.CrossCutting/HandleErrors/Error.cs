using System.Net;

namespace Railway.Platform.CrossCutting.HandleErrors
{
    public sealed record Error(HttpStatusCode Code, string Description) { }
}
