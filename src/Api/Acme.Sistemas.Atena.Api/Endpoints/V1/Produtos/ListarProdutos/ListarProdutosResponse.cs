using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.ListarProdutos;

public sealed record ListarProdutosResponseItem(
    Guid Id,
    string Codigo,
    string Nome,
    string? CodigoBarras,
    string UnidadeMedida,
    decimal? CustoMedio,
    StatusAtivo Status);

public sealed record ListarProdutosResponse(
    IReadOnlyList<ListarProdutosResponseItem> Items,
    long Total);
