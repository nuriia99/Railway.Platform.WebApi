using Railway.Platform.Domain.Events;
using Railway.Platform.UnitTests.Fixtures;

namespace Railway.Platform.UnitTests.Railway.Platform.Domain.Events
{
    /// <summary>
    /// Unit tests for EmailMessage record
    /// Validates proper creation and equality
    /// </summary>
    public class EmailMessageTests
    {
        [Fact]
        public void EmailMessage_ShouldBeCreated_WithValidData()
        {
            // Arrange
            var emailMessage = EmailMessageBuilder.Default().Build();

            // Act & Assert
            emailMessage.Should().NotBeNull();
            emailMessage.To.Should().Be("test@example.com");
            emailMessage.Subject.Should().Be("Test Subject");
            emailMessage.Body.Should().Be("Test Body");
        }

        [Fact]
        public void EmailMessage_ShouldBeEqualWhenDataMatches()
        {
            // Arrange
            var message1 = new EmailMessaje
            {
                To = "test@example.com",
                Subject = "Subject",
                Body = "Body"
            };

            var message2 = new EmailMessaje
            {
                To = "test@example.com",
                Subject = "Subject",
                Body = "Body"
            };

            // Act & Assert
            message1.Should().Be(message2);
        }
    }
}

