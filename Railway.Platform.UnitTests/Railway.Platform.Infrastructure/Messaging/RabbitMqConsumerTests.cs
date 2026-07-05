using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Railway.Platform.Application.Messaging;
using Railway.Platform.Infrastructure.Messaging;

namespace Railway.Platform.UnitTests.Railway.Platform.Infrastructure.Messaging
{
    /// <summary>
    /// Unit tests for RabbitMqConsumer
    /// Tests constructor initialization and injected dependencies
    /// Note: Full integration tests with RabbitMQ are excluded
    /// </summary>
    public class RabbitMqConsumerTests
    {
        [Fact]
        public void RabbitMqConsumer_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var configSubstitute = Substitute.For<IConfiguration>();
            var messageDispatcherSubstitute = Substitute.For<IMessageDispatcher>();
            var loggerSubstitute = Substitute.For<ILogger<RabbitMqConsumer>>();

            // Act
            var consumer = new RabbitMqConsumer(
                configSubstitute,
                messageDispatcherSubstitute,
                loggerSubstitute
            );

            // Assert
            consumer.Should().NotBeNull();
        }
    }
}
