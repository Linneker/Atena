namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.CriarSolicitacaoCompra;

public sealed record CriarSolicitacaoCompraRequestItem(
    Guid ProdutoId,
    decimal Quantidade,
    decimal? PrecoEstimado,
    string? Observacao);

public sealed record CriarSolicitacaoCompraRequest(
    string? Justificativa,
    IReadOnlyList<CriarSolicitacaoCompraRequestItem> Itens,
    bool EnviarParaAprovacao = false);
