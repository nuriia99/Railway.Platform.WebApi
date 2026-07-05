using Railway.Platform.Application.Messaging;
using Railway.Platform.Domain.Enums;
using Railway.Platform.Infrastructure.Messaging;
using Railway.Platform.UnitTests.Helpers;

namespace Railway.Platform.UnitTests.Railway.Platform.Application.Messaging
{
    /// <summary>
    /// Unit tests for MessageDispatcher
    /// Tests message routing and handler dispatch logic
    /// </summary>
    public class MessageDispatcherTests
    {
        private readonly IMessageDispatcher _dispatcher;
        private readonly IMessageHandler _handlerSubstitute1;
        private readonly IMessageHandler _handlerSubstitute2;

        public MessageDispatcherTests()
        {
            _handlerSubstitute1 = Substitute.For<IMessageHandler>();
            _handlerSubstitute2 = Substitute.For<IMessageHandler>();

            _handlerSubstitute1.MessageType.Returns("Handler1Type");
            _handlerSubstitute2.MessageType.Returns("Handler2Type");

            _handlerSubstitute1.HandleAsync(Arg.Any<System.Text.Json.JsonElement>(), Arg.Any<int>())
                .Returns(x => Task.FromResult(new HandlerResult { Status = HandlerResultStatus.Success }));

            _handlerSubstitute2.HandleAsync(Arg.Any<System.Text.Json.JsonElement>(), Arg.Any<int>())
                .Returns(x => Task.FromResult(new HandlerResult { Status = HandlerResultStatus.Retry }));

            _dispatcher = new MessageDispatcher(new[] { _handlerSubstitute1, _handlerSubstitute2 });
        }

        [Fact]
        public async Task DispatchAsync_ShouldDispatchToCorrectHandler()
        {
            // Arrange
            var json = JsonGenerator.LoadJson("dispatcher-handler1.json");

            // Act
            var result = await _dispatcher.DispatchAsync(json, 0);

            // Assert
            result.Status.Should().Be(HandlerResultStatus.Success);
            await _handlerSubstitute1.Received(1).HandleAsync(
                Arg.Any<System.Text.Json.JsonElement>(),
                Arg.Any<int>()
            );
        }

        [Fact]
        public async Task DispatchAsync_ShouldRespectRetryCount()
        {
            // Arrange
            var json = JsonGenerator.LoadJson("dispatcher-empty.json");

            // Act
            await _dispatcher.DispatchAsync(json, 2);

            // Assert
            await _handlerSubstitute1.Received(1).HandleAsync(
                Arg.Any<System.Text.Json.JsonElement>(),
                Arg.Is(2)
            );
        }

        [Fact]
        public async Task DispatchAsync_ShouldThrowException_WhenHandlerNotFound()
        {
            // Arrange
            var json = JsonGenerator.LoadJson("dispatcher-unknown.json");

            // Act & Assert
            var ex = await Record.ExceptionAsync(() => _dispatcher.DispatchAsync(json, 0));
            ex.Should().NotBeNull();
            ex!.Message.Should().Contain("No handler found");
        }
    }
}
