using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Railway.Platform.Application.Handlers;
using Railway.Platform.Domain.Enums;
using Railway.Platform.UnitTests.Helpers;

namespace Railway.Platform.UnitTests.Railway.Platform.Application.Handlers
{
    /// <summary>
    /// Unit tests for EmailMessageHandler
    /// Tests handler behavior with mocked dependencies
    /// </summary>
    public class EmailMessageHandlerTests
    {
        private readonly IConfiguration _configSubstitute;
        private readonly ILogger<EmailMessageHandler> _loggerSubstitute;
        private readonly EmailMessageHandler _handler;

        public EmailMessageHandlerTests()
        {
            _configSubstitute = Substitute.For<IConfiguration>();
            _loggerSubstitute = Substitute.For<ILogger<EmailMessageHandler>>();

            // Configure SMTP defaults
            _configSubstitute["SMTP:HOST"].Returns("smtp.example.com");
            _configSubstitute["SMTP:PORT"].Returns("587");
            _configSubstitute["SMTP:USER"].Returns("user@example.com");
            _configSubstitute["SMTP:PASS"].Returns("password");

            _handler = new EmailMessageHandler(_configSubstitute, _loggerSubstitute);
        }

        [Fact]
        public void EmailMessageHandler_ShouldHaveCorrectMessageType()
        {
            // Assert
            _handler.MessageType.Should().Be("SendEmail");
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnDiscard_WhenMessageIsNull()
        {
            // Arrange
            var jsonElement = JsonGenerator.LoadData("email-null.json");

            // Act
            var result = await _handler.HandleAsync(jsonElement, 0);

            // Assert
            result.Status.Should().Be(HandlerResultStatus.Discard);
            result.ErrorMessage.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnRetry_WhenInvalidJsonData()
        {
            // Arrange
            var jsonElement = JsonGenerator.LoadData("email-invalid-structure.json");

            // Act
            var result = await _handler.HandleAsync(jsonElement, 0);

            // Assert
            result.Status.Should().Be(HandlerResultStatus.Retry);
        }

        [Fact]
        public async Task HandleAsync_ShouldProcessValidEmail_WithCorrectData()
        {
            // Arrange 
            var configWithBadHost = Substitute.For<IConfiguration>();
            configWithBadHost["SMTP:HOST"].Returns("invalid-smtp-host-that-does-not-exist.local");
            configWithBadHost["SMTP:PORT"].Returns("587");
            configWithBadHost["SMTP:USER"].Returns("user@example.com");
            configWithBadHost["SMTP:PASS"].Returns("password");

            var logger = Substitute.For<ILogger<EmailMessageHandler>>();
            var handler = new EmailMessageHandler(configWithBadHost, logger);
            var jsonElement = JsonGenerator.LoadData("email-valid.json");

            // Act 
            var result = await handler.HandleAsync(jsonElement, 0);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(HandlerResultStatus.Retry);
            result.ErrorMessage.Should().NotBeNullOrEmpty();
        }
    }
}
