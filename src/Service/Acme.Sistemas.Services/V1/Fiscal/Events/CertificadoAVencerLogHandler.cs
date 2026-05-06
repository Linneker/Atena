using Acme.Sistemas.Core.Mediators.Notification;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Services.V1.Fiscal.Events;

public sealed class CertificadoAVencerLogHandler : INotificationHandler<CertificadoAVencerNotification>
{
    private readonly ILogger<CertificadoAVencerLogHandler> _logger;
    public CertificadoAVencerLogHandler(ILogger<CertificadoAVencerLogHandler> logger) => _logger = logger;

    public Task Handle(CertificadoAVencerNotification n, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Certificado a vencer: tenant={Tenant} subject={Subject} validoAte={Validade} dias={Dias}",
            n.TenantId, n.Subject, n.ValidoAte, n.DiasRestantes);
        return Task.CompletedTask;
    }
}
