namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.ListarLotacoes;

public sealed record ListarLotacoesResponseItem(
    Guid Id,
    string Nome,
    Guid? EmpresaId,
    string? Cnpj,
    bool Ativo);

public sealed record ListarLotacoesResponse(
    IReadOnlyList<ListarLotacoesResponseItem> Items,
    long Total);
