using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Railway.Platform.Application.Interfaces;
using Railway.Platform.Application.Messaging;
using Railway.Platform.Domain.Interfaces;
using Railway.Platform.Infrastructure.Data;
using Railway.Platform.Infrastructure.Messaging;
using Railway.Platform.Infrastructure.Repositories;
using Railway.Platform.Infrastructure.Services;

namespace Railway.Platform.Infrastructure.Configuration
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IMessageDispatcher, MessageDispatcher>();
            services.AddHostedService<RabbitMqConsumer>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordService, PasswordService>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(config["Database:ConnectionString"]);
            });

            return services;
        }
    }
}
