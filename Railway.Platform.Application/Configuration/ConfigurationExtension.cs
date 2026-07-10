using Microsoft.Extensions.DependencyInjection;
using Railway.Platform.Application.Handlers;
using Railway.Platform.Application.Interfaces;
using Railway.Platform.Application.Messaging;
using Railway.Platform.Application.Services;

namespace Railway.Platform.Application.Configuration
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IMessageHandler, MessageTestHandler>();
            services.AddScoped<IMessageHandler, EmailMessageHandler>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
