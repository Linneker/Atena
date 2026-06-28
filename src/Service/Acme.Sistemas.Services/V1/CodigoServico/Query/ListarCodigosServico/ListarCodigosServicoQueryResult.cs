namespace Acme.Sistemas.Services.V1.CodigoServico.Query.ListarCodigosServico;

public sealed record ListarCodigosServicoQueryItem(string Codigo, string Descricao);

public sealed record ListarCodigosServicoQueryResult(IReadOnlyList<ListarCodigosServicoQueryItem> Items);
