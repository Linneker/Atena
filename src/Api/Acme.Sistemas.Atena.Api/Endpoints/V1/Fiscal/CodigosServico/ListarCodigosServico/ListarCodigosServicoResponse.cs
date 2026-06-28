namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fiscal.CodigosServico.ListarCodigosServico;

public sealed record ListarCodigosServicoResponseItem(string Codigo, string Descricao);

public sealed record ListarCodigosServicoResponse(IReadOnlyList<ListarCodigosServicoResponseItem> Items);
