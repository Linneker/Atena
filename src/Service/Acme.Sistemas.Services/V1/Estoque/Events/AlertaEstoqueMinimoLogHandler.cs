using Acme.Sistemas.Core.Mediators.Notification;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Services.V1.Estoque.Events;

/// <summary>
/// Handler default: registra log estruturado.
/// Outros handlers (push, e-mail, dashboard real-time) podem ser adicionados livremente
/// — todos os handlers registrados para essa notificação rodam em paralelo.
/// </summary>
public sealed class AlertaEstoqueMinimoLogHandler : INotificationHandler<AlertaEstoqueMinimoNotification>
{
    private readonly ILogger<AlertaEstoqueMinimoLogHandler> _logger;

    public AlertaEstoqueMinimoLogHandler(ILogger<AlertaEstoqueMinimoLogHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(AlertaEstoqueMinimoNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Estoque mínimo atingido: tenant={Tenant} estoque={Estoque} produto={Produto} saldo={Saldo} minimo={Minimo}",
            notification.TenantId,
            notification.EstoqueId,
            notification.ProdutoId,
            notification.SaldoAtual,
            notification.EstoqueMinimo);
        return Task.CompletedTask;
    }
}
