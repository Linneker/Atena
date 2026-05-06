using Acme.Sistemas.Core.Mediators.Notification;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Services.V1.Faturamento.Events;

public sealed class NFeEmissaoSolicitadaLogHandler : INotificationHandler<NFeEmissaoSolicitadaNotification>
{
    private readonly ILogger<NFeEmissaoSolicitadaLogHandler> _logger;
    public NFeEmissaoSolicitadaLogHandler(ILogger<NFeEmissaoSolicitadaLogHandler> logger) => _logger = logger;

    public Task Handle(NFeEmissaoSolicitadaNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "NF-e solicitada: tenant={Tenant} origem={Origem} operacao={Op} valor={Valor}",
            notification.TenantId, notification.OrigemId, notification.Operacao, notification.ValorTotal);
        return Task.CompletedTask;
    }
}
