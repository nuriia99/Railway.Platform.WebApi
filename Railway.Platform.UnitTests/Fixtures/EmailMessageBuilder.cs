using Railway.Platform.Domain.Events;

namespace Railway.Platform.UnitTests.Fixtures
{
    /// <summary>
    /// Builder for creating EmailMessaje test instances
    /// Implements the Fluent Builder pattern for cleaner test setup
    /// </summary>
    public class EmailMessageBuilder
    {
        private string _to = "test@example.com";
        private string _subject = "Test Subject";
        private string _body = "Test Body";

        public EmailMessageBuilder WithTo(string to)
        {
            _to = to;
            return this;
        }

        public EmailMessageBuilder WithSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public EmailMessageBuilder WithBody(string body)
        {
            _body = body;
            return this;
        }

        public EmailMessaje Build()
        {
            return new EmailMessaje
            {
                To = _to,
                Subject = _subject,
                Body = _body
            };
        }

        public static EmailMessageBuilder Default() => new();
    }
}
