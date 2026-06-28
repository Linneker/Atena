namespace Acme.Sistemas.Services.V1.Cfop.Query.ListarCfops;

public sealed record ListarCfopsQueryItem(string Codigo, string Descricao, string Categoria);

public sealed record ListarCfopsQueryResult(IReadOnlyList<ListarCfopsQueryItem> Items);
