namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarPoliticas;

public sealed record ListarPoliticasQueryItem(
    Guid Id, string Nome, decimal LimiteHorasAcumular,
    int PrazoCompensacaoDias, bool PermitePagarExcedente, bool Ativo);

public sealed record ListarPoliticasQueryResult(IReadOnlyList<ListarPoliticasQueryItem> Items, long Total);
