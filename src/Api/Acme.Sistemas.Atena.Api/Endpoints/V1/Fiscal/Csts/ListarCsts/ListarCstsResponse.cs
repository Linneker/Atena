namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fiscal.Csts.ListarCsts;

public sealed record ListarCstsResponseItem(string Codigo, string Descricao);

public sealed record ListarCstsResponse(string Tipo, IReadOnlyList<ListarCstsResponseItem> Items);
