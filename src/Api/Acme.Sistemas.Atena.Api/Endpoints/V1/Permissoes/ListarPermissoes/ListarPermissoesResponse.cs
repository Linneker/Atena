namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Permissoes.ListarPermissoes;

public sealed record ListarPermissoesResponseItem(string Codigo, string Recurso, string Acao, string? Descricao);

public sealed record ListarPermissoesResponse(IReadOnlyList<ListarPermissoesResponseItem> Items);
