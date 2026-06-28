namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.ListarDepartamentos;

public sealed record ListarDepartamentosResponseItem(
    Guid Id,
    string? Codigo,
    string Nome,
    Guid? CentroDeCustoId,
    bool Ativo);

public sealed record ListarDepartamentosResponse(
    IReadOnlyList<ListarDepartamentosResponseItem> Items,
    long Total);
