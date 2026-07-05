using Railway.Platform.Domain.Events;
using Railway.Platform.UnitTests.Fixtures;

namespace Railway.Platform.UnitTests.Railway.Platform.Domain.Events
{
    /// <summary>
    /// Unit tests for MessageTest record
    /// Validates proper creation and equality
    /// </summary>
    public class MessageTestTests
    {
        [Fact]
        public void MessageTest_ShouldBeCreated_WithValidData()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var amount = 150.50m;

            // Act
            var message = MessageTestBuilder.Default()
                .WithMessageId(messageId)
                .WithAmount(amount)
                .Build();

            // Assert
            message.MessageId.Should().Be(messageId);
            message.Amount.Should().Be(amount);
        }

        [Fact]
        public void MessageTest_ShouldBeEqualWhenDataMatches()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var message1 = new MessageTest
            {
                MessageId = messageId,
                Amount = 100m
            };

            var message2 = new MessageTest
            {
                MessageId = messageId,
                Amount = 100m
            };

            // Act & Assert
            message1.Should().Be(message2);
        }

        [Fact]
        public void MessageTest_ShouldNotBeEqual_WithDifferentAmounts()
        {
            // Arrange
            var message1 = MessageTestBuilder.Default().Build();
            var message2 = MessageTestBuilder.Default()
                .WithAmount(200m)
                .Build();

            // Act & Assert
            message1.Should().NotBe(message2);
        }
    }
}
