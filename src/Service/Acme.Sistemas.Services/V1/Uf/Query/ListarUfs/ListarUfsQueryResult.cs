namespace Acme.Sistemas.Services.V1.Uf.Query.ListarUfs;

public sealed record ListarUfsQueryItem(string Sigla, string Nome, int CodigoIbge);

public sealed record ListarUfsQueryResult(IReadOnlyList<ListarUfsQueryItem> Items);
