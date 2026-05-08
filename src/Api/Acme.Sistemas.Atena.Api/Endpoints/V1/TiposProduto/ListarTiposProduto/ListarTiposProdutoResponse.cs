namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposProduto.ListarTiposProduto;

public sealed record ListarTiposProdutoResponseItem(Guid Id, string Nome, string? Descricao, bool Ativo);

public sealed record ListarTiposProdutoResponse(IReadOnlyList<ListarTiposProdutoResponseItem> Items);
