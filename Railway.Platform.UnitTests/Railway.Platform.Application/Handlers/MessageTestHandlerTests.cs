using Railway.Platform.Application.Handlers;
using Railway.Platform.Domain.Enums;
using Railway.Platform.UnitTests.Helpers;

namespace Railway.Platform.UnitTests.Railway.Platform.Application.Handlers
{
    /// <summary>
    /// Unit tests for MessageTestHandler
    /// Tests handler behavior with different message scenarios
    /// </summary>
    public class MessageTestHandlerTests
    {
        private readonly MessageTestHandler _handler;

        public MessageTestHandlerTests()
        {
            _handler = new MessageTestHandler();
        }

        [Fact]
        public void MessageTestHandler_ShouldHaveCorrectMessageType()
        {
            // Assert
            _handler.MessageType.Should().Be("MessageTest");
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WithValidData()
        {
            // Arrange
            var jsonElement = JsonGenerator.LoadData("messagetest-valid.json");

            // Act
            var result = await _handler.HandleAsync(jsonElement, 0);

            // Assert
            result.Status.Should().Be(HandlerResultStatus.Success);
        }
    }
}
