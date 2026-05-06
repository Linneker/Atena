namespace Acme.Sistemas.Infrastructure.Messaging.RabbitMq;

public interface IRabbitMqBus
{
    Task PublishAsync<T>(string exchange, string routingKey, T payload, CancellationToken cancellationToken = default);
    Task SubscribeAsync<T>(string queue, Func<T, CancellationToken, Task> handler, CancellationToken cancellationToken = default);
}
