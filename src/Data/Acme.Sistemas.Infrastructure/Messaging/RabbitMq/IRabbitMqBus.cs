namespace Acme.Sistemas.Infrastructure.Messaging.RabbitMq;

public interface IRabbitMqBus
{
    Task PublishAsync<T>(string exchange, string routingKey, T payload, CancellationToken cancellationToken = default);

    Task SubscribeAsync<T>(string queue, Func<T, CancellationToken, Task> handler, CancellationToken cancellationToken = default);

    Task SubscribeBoundAsync<T>(
        SubscribeBinding binding,
        Func<T, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default);
}

public sealed record SubscribeBinding(
    string Exchange,
    string Queue,
    string RoutingKey,
    string DeadLetterExchange,
    string DeadLetterQueue,
    string DeadLetterRoutingKey,
    int MaxRetries = 5,
    ushort PrefetchCount = 10);
