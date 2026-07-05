using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Railway.Platform.Application.Messaging;
using Railway.Platform.Infrastructure.Messaging;

namespace Railway.Platform.UnitTests.Railway.Platform.Infrastructure.Messaging
{
    public class RabbitMqConsumerTests
    {
        [Fact]
        public void RabbitMqConsumer_Constructor_ShouldInitializeCorrectly()
        {
            var configSubstitute = Substitute.For<IConfiguration>();
            var scopeFactorySubstitute = Substitute.For<IServiceScopeFactory>();
            var loggerSubstitute = Substitute.For<ILogger<RabbitMqConsumer>>();

            var consumer = new RabbitMqConsumer(
                configSubstitute,
                scopeFactorySubstitute,
                loggerSubstitute
            );

            consumer.Should().NotBeNull();
        }
    }
}
