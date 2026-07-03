using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Railway.EventConsumer.Application.Messaging;
using Railway.EventConsumer.Domain.Enums;
using System.Text;

namespace Railway.EventConsumer.Infrastructure.Messaging
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly IMessageDispatcher _messageDispatcher;

        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMqConsumer(
            IConfiguration config,
            IMessageDispatcher messageDispatcher,
            ILogger<RabbitMqConsumer> logger)
        {
            _config = config;
            _logger = logger;
            _messageDispatcher = messageDispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionString = _config["RabbitMQ:ConnectionString"];
            var queueName = _config["RabbitMQ:QueueName"];

            var factory = new ConnectionFactory { Uri = new Uri(connectionString!) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, args) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(args.Body.ToArray());

                    var headers = args.BasicProperties.Headers ?? new Dictionary<string, object>();
                    int retryCount = headers.ContainsKey("x-retry-count")
                        ? Convert.ToInt32(headers["x-retry-count"])
                        : 0;

                    var result = await _messageDispatcher.DispatchAsync(json, retryCount);

                    HandleResult(args, _channel, result, retryCount);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing message");
                    _channel.BasicNack(args.DeliveryTag, false, false);
                }
            };

            _channel.BasicConsume(queueName, false, consumer);
            _logger.LogInformation("Consumer is listening...");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private void HandleResult(
            BasicDeliverEventArgs args,
            IModel channel,
            HandlerResult result,
            int retryCount)
        {
            switch (result.Status)
            {
                case HandlerResultStatus.Success:
                    channel.BasicAck(args.DeliveryTag, false);
                    return;

                case HandlerResultStatus.Retry:
                    RetryMessage(args, channel, retryCount);
                    return;

                case HandlerResultStatus.Discard:
                    channel.BasicNack(args.DeliveryTag, false, false);
                    _logger.LogError("Message discarded by handler decision");
                    return;
            }
        }

        private void RetryMessage(BasicDeliverEventArgs args, IModel channel, int retryCount)
        {
            if (retryCount >= 3)
            {
                channel.BasicNack(args.DeliveryTag, false, false);
                _logger.LogError("Message discarded after 3 retries");
                return;
            }

            var props = channel.CreateBasicProperties();
            props.Headers = args.BasicProperties.Headers ?? new Dictionary<string, object>();
            props.Headers["x-retry-count"] = retryCount + 1;

            channel.BasicPublish(
                exchange: "retry-exchange",
                routingKey: "retry",
                basicProperties: props,
                body: args.Body);

            channel.BasicAck(args.DeliveryTag, false);

            _logger.LogWarning($"Message sent to retry: {retryCount + 1}");
        }
    }
}
