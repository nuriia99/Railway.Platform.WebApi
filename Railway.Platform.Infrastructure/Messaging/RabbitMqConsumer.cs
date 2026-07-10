using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Railway.Platform.Application.Messaging;
using Railway.Platform.Domain.Enums;
using System.Text;

namespace Railway.Platform.Infrastructure.Messaging
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMqConsumer(
            IConfiguration config,
            IServiceScopeFactory scopeFactory,
            ILogger<RabbitMqConsumer> logger)
        {
            _config = config;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var connectionString = _config["RabbitMQ:ConnectionString"];
            var queueName = _config["RabbitMQ:QueueName"];

            var factory = new ConnectionFactory { Uri = new Uri(connectionString!) };
            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(args.Body.ToArray());

                    var headers = args.BasicProperties.Headers ?? new Dictionary<string, object?>();
                    int retryCount = headers.TryGetValue("x-retry-count", out var val) && val is int intVal
                        ? intVal
                        : 0;

                    using var scope = _scopeFactory.CreateScope();
                    var dispatcher = scope.ServiceProvider.GetRequiredService<IMessageDispatcher>();
                    var result = await dispatcher.DispatchAsync(json, retryCount);

                    await HandleResultAsync(args, _channel!, result, retryCount, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing message");
                    await _channel!.BasicNackAsync(args.DeliveryTag, false, false, cancellationToken);
                }
            };

            await _channel.BasicConsumeAsync(queueName!, false, consumer, cancellationToken: cancellationToken);
            _logger.LogInformation("Consumer is listening...");

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        private async Task HandleResultAsync(
            BasicDeliverEventArgs args,
            IChannel channel,
            HandlerResult result,
            int retryCount,
            CancellationToken stoppingToken)
        {
            switch (result.Status)
            {
                case HandlerResultStatus.Success:
                    await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
                    return;

                case HandlerResultStatus.Retry:
                    await RetryMessageAsync(args, channel, retryCount, stoppingToken);
                    return;

                case HandlerResultStatus.Discard:
                    await channel.BasicNackAsync(args.DeliveryTag, false, false, stoppingToken);
                    _logger.LogError("Message discarded by handler decision");
                    return;
            }
        }

        private async Task RetryMessageAsync(BasicDeliverEventArgs args, IChannel channel, int retryCount, CancellationToken stoppingToken)
        {
            if (retryCount >= 3)
            {
                await channel.BasicNackAsync(args.DeliveryTag, false, false, stoppingToken);
                _logger.LogError("Message discarded after 3 retries");
                return;
            }

            var props = new BasicProperties();
            props.Headers = args.BasicProperties.Headers ?? new Dictionary<string, object?>();
            props.Headers["x-retry-count"] = retryCount + 1;

            await channel.BasicPublishAsync(
                exchange: "retry-exchange",
                routingKey: "retry",
                mandatory: false,
                basicProperties: props,
                body: args.Body,
                cancellationToken: stoppingToken);

            await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);

            _logger.LogWarning($"Message sent to retry: {retryCount + 1}");
        }
    }
}
