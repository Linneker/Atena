using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Roles.Query.ListarPermissoes;

public sealed record ListarPermissoesQueryItem(string Codigo, string Recurso, string Acao, string? Descricao);

public sealed record ListarPermissoesQueryResult(IReadOnlyList<ListarPermissoesQueryItem> Items);
