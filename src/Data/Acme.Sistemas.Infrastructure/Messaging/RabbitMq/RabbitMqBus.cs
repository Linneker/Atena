using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Acme.Sistemas.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqBus : IRabbitMqBus, IDisposable
{
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

    public void Dispose()
    {
        if (_connection.IsValueCreated)
        {
            _connection.Value.Close();
            _connection.Value.Dispose();
        }
    }
}
