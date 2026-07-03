using Microsoft.Extensions.DependencyInjection;
using Railway.EventConsumer.Application.Handlers;
using Railway.EventConsumer.Application.Messaging;

namespace Railway.EventConsumer.Application.Configuration
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
