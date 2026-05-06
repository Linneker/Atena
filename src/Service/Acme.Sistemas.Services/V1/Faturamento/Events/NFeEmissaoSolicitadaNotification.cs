using Acme.Sistemas.Core.Mediators.Notification;

namespace Acme.Sistemas.Services.V1.Faturamento.Events;

public enum NFeOperacao
{
    Saida = 1,
    Devolucao = 2
}

public sealed record NFeEmissaoSolicitadaNotification(
    Guid TenantId,
    Guid OrigemId,
    NFeOperacao Operacao,
    decimal ValorTotal,
    DateTime SolicitadoEm) : INotification;
