namespace Acme.Sistemas.Domain.Interfaces.Fiscal;

public interface INFeTransmissaoEnqueuer
{
    Task EnqueueAsync(Guid tenantId, Guid nfeId, CancellationToken cancellationToken = default);
}
