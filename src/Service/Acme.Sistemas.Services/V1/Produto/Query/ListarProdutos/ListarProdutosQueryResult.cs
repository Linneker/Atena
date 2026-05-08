using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Produto.Query.ListarProdutos;

public sealed record ListarProdutosQueryItem(
    Guid Id, string Codigo, string Nome, string? CodigoBarras,
    string UnidadeMedida, decimal? CustoMedio, StatusAtivo Status);

public sealed record ListarProdutosQueryResult(IReadOnlyList<ListarProdutosQueryItem> Items, long Total);
