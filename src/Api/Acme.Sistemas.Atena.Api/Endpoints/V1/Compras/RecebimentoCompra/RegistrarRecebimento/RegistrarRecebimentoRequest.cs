namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.RecebimentoCompra.RegistrarRecebimento;

public sealed record RegistrarRecebimentoRequestItem(
    Guid PedidoCompraItemId,
    decimal QuantidadeRecebida,
    decimal? PrecoUnitario,
    string? Observacao);

public sealed record RegistrarRecebimentoRequest(
    Guid PedidoCompraId,
    Guid EstoqueId,
    DateTime? DataRecebimento,
    string? NumeroNotaFiscal,
    string? ChaveAcessoNFe,
    string? Observacao,
    DateTime VencimentoContaPagar,
    Guid? PlanoDeContasId,
    IReadOnlyList<RegistrarRecebimentoRequestItem> Itens);
