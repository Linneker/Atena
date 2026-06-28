namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ListarLotacoes;

public sealed record ListarLotacoesQueryItem(
    Guid Id,
    string Nome,
    Guid? EmpresaId,
    string? Cnpj,
    bool Ativo);

public sealed record ListarLotacoesQueryResult(
    IReadOnlyList<ListarLotacoesQueryItem> Items,
    long Total);
