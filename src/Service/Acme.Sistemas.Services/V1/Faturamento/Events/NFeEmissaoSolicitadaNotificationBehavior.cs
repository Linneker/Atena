using Acme.Sistemas.Core.Mediators.Notification;

namespace Acme.Sistemas.Services.V1.Faturamento.Events;

/// <summary>
/// Behavior específico do NFeEmissaoSolicitadaNotification. Notification não tem pipeline genérico
/// (publish é fire-and-forget), mas a convenção do blueprint exige o arquivo Behavior.cs
/// no mesmo namespace para futura extensibilidade (ex.: enriquecimento de payload, dedup).
/// </summary>
public static class NFeEmissaoSolicitadaNotificationBehavior
{
    // Sem pipeline ativo — placeholder de convenção. Substituído por hooks reais quando necessário.
}
