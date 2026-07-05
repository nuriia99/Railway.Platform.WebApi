using Railway.Platform.Application.Messaging;
using Railway.Platform.Domain.Enums;

namespace Railway.Platform.UnitTests.Railway.Platform.Application.Messaging
{
    /// <summary>
    /// Unit tests for HandlerResult
    /// Tests result creation and status management
    /// </summary>
    public class HandlerResultTests
    {
        [Fact]
        public void HandlerResult_ShouldBeCreated_WithSuccessStatus()
        {
            // Arrange & Act
            var result = new HandlerResult
            {
                Status = HandlerResultStatus.Success,
                ErrorMessage = null
            };

            // Assert
            result.Status.Should().Be(HandlerResultStatus.Success);
            result.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public void HandlerResult_ShouldBeCreated_WithRetryStatus()
        {
            // Arrange & Act
            var result = new HandlerResult
            {
                Status = HandlerResultStatus.Retry,
                ErrorMessage = "Temporary error"
            };

            // Assert
            result.Status.Should().Be(HandlerResultStatus.Retry);
            result.ErrorMessage.Should().Be("Temporary error");
        }

        [Fact]
        public void HandlerResult_ShouldBeCreated_WithDiscardStatus()
        {
            // Arrange & Act
            var result = new HandlerResult
            {
                Status = HandlerResultStatus.Discard,
                ErrorMessage = "Invalid message"
            };

            // Assert
            result.Status.Should().Be(HandlerResultStatus.Discard);
        }
    }
}
