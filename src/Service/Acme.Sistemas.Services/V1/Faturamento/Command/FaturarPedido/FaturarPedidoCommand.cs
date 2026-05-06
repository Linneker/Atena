using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Faturamento.Command.FaturarPedido;

public sealed record FaturarPedidoItemDto(Guid PedidoVendaItemId, decimal Quantidade);

public sealed record FaturarPedidoCommand(
    Guid PedidoVendaId,
    DateTime VencimentoContaReceber,
    Guid? PlanoDeContasId,
    decimal? PercentualComissaoOverride,
    IReadOnlyList<FaturarPedidoItemDto> Itens) : IRequest<ResponseDefault<FaturarPedidoCommandResult>>;

public sealed record FaturarPedidoCommandResult(
    Guid FaturamentoId,
    string Numero,
    decimal ValorTotal,
    Guid? ContaReceberId,
    Guid? ComissaoId,
    bool NFeSolicitada);
