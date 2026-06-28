namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.CriarPedidoCompra;

public sealed record CriarPedidoCompraRequestItem(
    Guid ProdutoId,
    decimal Quantidade,
    decimal PrecoUnitario);

public sealed record CriarPedidoCompraRequest(
    Guid FornecedorId,
    Guid? SolicitacaoCompraId,
    DateTime? PrevisaoEntrega,
    string? CondicaoPagamento,
    string? Observacao,
    IReadOnlyList<CriarPedidoCompraRequestItem>? Itens);
