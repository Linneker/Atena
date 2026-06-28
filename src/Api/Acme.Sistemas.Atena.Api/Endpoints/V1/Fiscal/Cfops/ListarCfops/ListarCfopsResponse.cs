namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fiscal.Cfops.ListarCfops;

public sealed record ListarCfopsResponseItem(string Codigo, string Descricao, string Categoria);

public sealed record ListarCfopsResponse(IReadOnlyList<ListarCfopsResponseItem> Items);
