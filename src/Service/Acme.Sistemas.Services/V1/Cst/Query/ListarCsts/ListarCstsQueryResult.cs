namespace Acme.Sistemas.Services.V1.Cst.Query.ListarCsts;

public sealed record ListarCstsQueryItem(string Codigo, string Descricao);

public sealed record ListarCstsQueryResult(string Tipo, IReadOnlyList<ListarCstsQueryItem> Items);
