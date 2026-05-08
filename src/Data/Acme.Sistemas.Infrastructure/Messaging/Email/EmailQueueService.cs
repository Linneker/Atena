using Acme.Sistemas.Domain.Interfaces.Messaging;
using Acme.Sistemas.Infrastructure.Messaging.RabbitMq;

namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public sealed class EmailQueueService : IEmailQueueService
{
    private readonly IRabbitMqBus _bus;

    public EmailQueueService(IRabbitMqBus bus)
    {
        _bus = bus;
    }

    public Task EnqueueAsync(EmailMessage message, CancellationToken cancellationToken = default)
        => _bus.PublishAsync(EmailQueueNames.Exchange, EmailQueueNames.RoutingKey, message, cancellationToken);
}
