namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ListarDepartamentos;

public sealed record ListarDepartamentosQueryItem(
    Guid Id,
    string? Codigo,
    string Nome,
    Guid? CentroDeCustoId,
    bool Ativo);

public sealed record ListarDepartamentosQueryResult(
    IReadOnlyList<ListarDepartamentosQueryItem> Items,
    long Total);
