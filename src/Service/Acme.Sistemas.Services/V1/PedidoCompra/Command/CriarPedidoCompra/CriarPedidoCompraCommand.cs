using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.CriarPedidoCompra;

public sealed record PedidoCompraItemDto(Guid ProdutoId, decimal Quantidade, decimal PrecoUnitario);

public sealed record CriarPedidoCompraCommand(
    Guid FornecedorId,
    Guid? SolicitacaoCompraId,
    DateTime? PrevisaoEntrega,
    string? CondicaoPagamento,
    string? Observacao,
    IReadOnlyList<PedidoCompraItemDto>? Itens) : IRequest<ResponseDefault<CriarPedidoCompraCommandResult>>;

public sealed record CriarPedidoCompraCommandResult(Guid Id, string Numero, decimal ValorTotal);
