using Railway.Platform.Domain.Events;

namespace Railway.Platform.UnitTests.Fixtures
{
    /// <summary>
    /// Builder for creating MessageTest test instances
    /// Implements the Fluent Builder pattern for cleaner test setup
    /// </summary>
    public class MessageTestBuilder
    {
        private Guid _messageId = Guid.NewGuid();
        private decimal _amount = 100m;

        public MessageTestBuilder WithMessageId(Guid messageId)
        {
            _messageId = messageId;
            return this;
        }

        public MessageTestBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }

        public MessageTest Build()
        {
            return new MessageTest
            {
                MessageId = _messageId,
                Amount = _amount
            };
        }

        public static MessageTestBuilder Default() => new();
    }
}
