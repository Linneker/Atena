using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.RegistrarRecebimento;

public sealed record RecebimentoItemDto(
    Guid PedidoCompraItemId,
    decimal QuantidadeRecebida,
    decimal? PrecoUnitario,
    string? Observacao);

