using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.TipoProduto.Query.ListarTiposProduto;

public sealed record ListarTiposProdutoQueryItem(Guid Id, string Nome, string? Descricao, bool Ativo);

public sealed record ListarTiposProdutoQueryResult(IReadOnlyList<ListarTiposProdutoQueryItem> Items);
