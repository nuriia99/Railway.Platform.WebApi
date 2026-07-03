using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Railway.Platform.Application.Messaging;
using Railway.Platform.Infrastructure.Messaging;

namespace Railway.Platform.Infrastructure.Configuration
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IMessageDispatcher, MessageDispatcher>();
            services.AddHostedService<RabbitMqConsumer>();
            return services;
        }
    }
}
