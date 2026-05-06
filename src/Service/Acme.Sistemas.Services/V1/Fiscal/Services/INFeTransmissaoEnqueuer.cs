namespace Acme.Sistemas.Services.V1.Fiscal.Services;

public interface INFeTransmissaoEnqueuer
{
    Task EnqueueAsync(Guid tenantId, Guid nfeId, CancellationToken cancellationToken = default);
}
