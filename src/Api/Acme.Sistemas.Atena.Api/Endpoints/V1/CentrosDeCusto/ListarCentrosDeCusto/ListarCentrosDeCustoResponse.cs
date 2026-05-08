namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.ListarCentrosDeCusto;

public sealed record ListarCentrosDeCustoResponseItem(
    Guid Id,
    string Codigo,
    string Nome,
    string? Descricao,
    Guid? ResponsavelId,
    bool Ativo);

public sealed record ListarCentrosDeCustoResponse(
    IReadOnlyList<ListarCentrosDeCustoResponseItem> Items);
