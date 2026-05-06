using Acme.Sistemas.Infrastructure.Messaging.RabbitMq;
using Acme.Sistemas.Services.V1.Fiscal.Services;

namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public sealed class NFeTransmissaoEnqueuer : INFeTransmissaoEnqueuer
{
    private readonly IRabbitMqBus _bus;

    public NFeTransmissaoEnqueuer(IRabbitMqBus bus)
    {
        _bus = bus;
    }

    public Task EnqueueAsync(Guid tenantId, Guid nfeId, CancellationToken cancellationToken = default)
        => _bus.PublishAsync(NFeQueueNames.Exchange, NFeQueueNames.RoutingKey,
            new NFeTransmissaoMessage(tenantId, nfeId), cancellationToken);
}
