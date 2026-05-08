namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposValorProduto.ListarTiposValorProduto;

public sealed record ListarTiposValorProdutoResponseItem(Guid Id, string Nome, string? Descricao, bool Ativo);

public sealed record ListarTiposValorProdutoResponse(IReadOnlyList<ListarTiposValorProdutoResponseItem> Items);
