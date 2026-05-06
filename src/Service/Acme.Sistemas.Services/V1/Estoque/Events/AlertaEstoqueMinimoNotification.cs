using Acme.Sistemas.Core.Mediators.Notification;

namespace Acme.Sistemas.Services.V1.Estoque.Events;

/// <summary>
/// Disparado quando o saldo disponível de um produto atinge o estoque mínimo após uma movimentação.
/// </summary>
public sealed record AlertaEstoqueMinimoNotification(
    Guid TenantId,
    Guid EstoqueId,
    Guid ProdutoId,
    decimal SaldoAtual,
    decimal? EstoqueMinimo,
    DateTime OcorridoEm) : INotification;
