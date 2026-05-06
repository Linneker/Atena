using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Acme.Sistemas.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqBus : IRabbitMqBus, IDisposable
{
    private const string RetryHeader = "x-acme-retry-count";

    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqBus> _logger;
    private readonly Lazy<IConnection> _connection;

    public RabbitMqBus(IOptions<RabbitMqOptions> options, ILogger<RabbitMqBus> logger)
    {
        _options = options.Value;
        _logger = logger;
        _connection = new Lazy<IConnection>(CreateConnection);
    }

    private IConnection CreateConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.Username,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost,
            DispatchConsumersAsync = _options.DispatchConsumersAsync
        };
        return factory.CreateConnection();
    }

    public Task PublishAsync<T>(string exchange, string routingKey, T payload, CancellationToken cancellationToken = default)
    {
        using var channel = _connection.Value.CreateModel();
        channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true, autoDelete: false);

        var body = JsonSerializer.SerializeToUtf8Bytes(payload);
        var props = channel.CreateBasicProperties();
        props.Persistent = true;
        props.ContentType = "application/json";
        props.MessageId = Guid.NewGuid().ToString();

        channel.BasicPublish(exchange, routingKey, props, body);
        _logger.LogDebug("Mensagem publicada em {Exchange}/{RoutingKey}", exchange, routingKey);
        return Task.CompletedTask;
    }

    public Task SubscribeAsync<T>(string queue, Func<T, CancellationToken, Task> handler, CancellationToken cancellationToken = default)
    {
        var channel = _connection.Value.CreateModel();
        channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (_, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var payload = JsonSerializer.Deserialize<T>(json);
                if (payload is null)
                {
                    channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }
                await handler(payload, cancellationToken);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem da fila {Queue}", queue);
                channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        channel.BasicConsume(queue, autoAck: false, consumer);
        return Task.CompletedTask;
    }

    public Task SubscribeBoundAsync<T>(
        SubscribeBinding binding,
        Func<T, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default)
    {
        var channel = _connection.Value.CreateModel();

        // Dead-letter topology: messages exceeding MaxRetries go here.
        channel.ExchangeDeclare(binding.DeadLetterExchange, ExchangeType.Topic, durable: true, autoDelete: false);
        channel.QueueDeclare(binding.DeadLetterQueue, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(binding.DeadLetterQueue, binding.DeadLetterExchange, binding.DeadLetterRoutingKey);

        // Main topology.
        channel.ExchangeDeclare(binding.Exchange, ExchangeType.Topic, durable: true, autoDelete: false);
        channel.QueueDeclare(binding.Queue, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(binding.Queue, binding.Exchange, binding.RoutingKey);

        channel.BasicQos(prefetchSize: 0, prefetchCount: binding.PrefetchCount, global: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (_, ea) =>
        {
            var retryCount = ReadRetryCount(ea.BasicProperties);

            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var payload = JsonSerializer.Deserialize<T>(json);
                if (payload is null)
                {
                    _logger.LogWarning("Mensagem inválida descartada da fila {Queue} (payload null).", binding.Queue);
                    PublishToDlq(channel, binding, ea, retryCount, "PAYLOAD_NULL");
                    channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }

                await handler(payload, cancellationToken);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro processando mensagem da fila {Queue} (tentativa {Retry}/{Max}).",
                    binding.Queue, retryCount + 1, binding.MaxRetries);

                if (retryCount + 1 >= binding.MaxRetries)
                {
                    PublishToDlq(channel, binding, ea, retryCount + 1, ex.GetType().Name);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    Republish(channel, binding, ea, retryCount + 1);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            }
        };

        channel.BasicConsume(binding.Queue, autoAck: false, consumer);
        return Task.CompletedTask;
    }

    private static int ReadRetryCount(IBasicProperties? props)
    {
        if (props?.Headers is null || !props.Headers.TryGetValue(RetryHeader, out var raw)) return 0;
        return raw switch
        {
            int i => i,
            long l => (int)l,
            byte[] bytes when int.TryParse(Encoding.UTF8.GetString(bytes), out var n) => n,
            _ => 0
        };
    }

    private static void Republish(IModel channel, SubscribeBinding binding, BasicDeliverEventArgs ea, int retryCount)
    {
        var props = channel.CreateBasicProperties();
        props.Persistent = true;
        props.ContentType = ea.BasicProperties?.ContentType ?? "application/json";
        props.MessageId = ea.BasicProperties?.MessageId ?? Guid.NewGuid().ToString();
        props.Headers = new Dictionary<string, object> { [RetryHeader] = retryCount };

        channel.BasicPublish(binding.Exchange, binding.RoutingKey, props, ea.Body);
    }

    private static void PublishToDlq(IModel channel, SubscribeBinding binding, BasicDeliverEventArgs ea, int retryCount, string reason)
    {
        var props = channel.CreateBasicProperties();
        props.Persistent = true;
        props.ContentType = ea.BasicProperties?.ContentType ?? "application/json";
        props.MessageId = ea.BasicProperties?.MessageId ?? Guid.NewGuid().ToString();
        props.Headers = new Dictionary<string, object>
        {
            [RetryHeader] = retryCount,
            ["x-acme-failure-reason"] = reason,
            ["x-acme-original-queue"] = binding.Queue
        };

        channel.BasicPublish(binding.DeadLetterExchange, binding.DeadLetterRoutingKey, props, ea.Body);
    }

    public void Dispose()
    {
        if (_connection.IsValueCreated)
        {
            _connection.Value.Close();
            _connection.Value.Dispose();
        }
    }
}
