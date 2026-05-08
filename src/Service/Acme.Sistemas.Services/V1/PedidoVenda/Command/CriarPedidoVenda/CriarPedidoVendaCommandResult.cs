using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Command.CriarPedidoVenda;

public sealed record CriarPedidoVendaCommand(
    Guid ClienteId,
    Guid? VendedorId,
    Guid EstoqueId,
    Guid? OrcamentoId,
    decimal? DescontoPercentual,
    string? CondicaoPagamento,
    string? Observacao,
    IReadOnlyList<PedidoVendaItemDto> Itens) : IRequest<ResponseDefault<CriarPedidoVendaCommandResult>>;

public sealed record CriarPedidoVendaCommandResult(Guid Id, string Numero, decimal ValorTotal);
