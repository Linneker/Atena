namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.CriarProduto;

public sealed record CriarProdutoRequest(
    string Codigo,
    string Nome,
    string? Descricao,
    string? CodigoBarras,
    string UnidadeMedida,
    Guid? TipoProdutoId,
    Guid? FornecedorId,
    decimal? CustoMedio,
    decimal? EstoqueMinimo);
