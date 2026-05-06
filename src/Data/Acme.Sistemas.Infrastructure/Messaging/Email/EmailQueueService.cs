using Acme.Sistemas.Infrastructure.Messaging.RabbitMq;

namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public sealed class EmailQueueService : IEmailQueueService
{
    private const string Exchange = "atena.email";
    private const string RoutingKey = "email.send";

    private readonly IRabbitMqBus _bus;

    public EmailQueueService(IRabbitMqBus bus)
    {
        _bus = bus;
    }

    public Task EnqueueAsync(EmailMessage message, CancellationToken cancellationToken = default)
        => _bus.PublishAsync(Exchange, RoutingKey, message, cancellationToken);
}
