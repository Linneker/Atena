using Acme.Sistemas.Core.Mediators.Notification;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Events;

public sealed class NotificarAprovacaoPendenteLogHandler : INotificationHandler<NotificarAprovacaoPendenteNotification>
{
    private readonly ILogger<NotificarAprovacaoPendenteLogHandler> _logger;

    public NotificarAprovacaoPendenteLogHandler(ILogger<NotificarAprovacaoPendenteLogHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(NotificarAprovacaoPendenteNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Solicitação de compra aguardando aprovação: tenant={Tenant} numero={Numero} valor={Valor} permissao_necessaria={Permissao}",
            notification.TenantId, notification.Numero, notification.ValorTotal, notification.PermissaoAprovacaoNecessaria);
        return Task.CompletedTask;
    }
}
