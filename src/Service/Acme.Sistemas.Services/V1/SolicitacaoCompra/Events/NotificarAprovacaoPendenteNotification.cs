using Acme.Sistemas.Core.Mediators.Notification;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Events;

public sealed record NotificarAprovacaoPendenteNotification(
    Guid TenantId,
    Guid SolicitacaoId,
    string Numero,
    Guid? SolicitanteId,
    decimal ValorTotal,
    string PermissaoAprovacaoNecessaria,
    DateTime DisparadoEm) : INotification;
