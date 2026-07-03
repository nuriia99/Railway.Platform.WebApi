using Microsoft.Extensions.DependencyInjection;
using Railway.Platform.Application.Handlers;
using Railway.Platform.Application.Messaging;

namespace Railway.Platform.Application.Configuration
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IMessageHandler, MessageTestHandler>();
            services.AddScoped<IMessageHandler, EmailMessageHandler>();

            return services;
        }
    }
}
