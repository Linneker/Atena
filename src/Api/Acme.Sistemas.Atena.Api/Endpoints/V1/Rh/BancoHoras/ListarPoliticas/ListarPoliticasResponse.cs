namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.ListarPoliticas;

public sealed record ListarPoliticasResponseItem(
    Guid Id, string Nome, decimal LimiteHorasAcumular,
    int PrazoCompensacaoDias, bool PermitePagarExcedente, bool Ativo);

public sealed record ListarPoliticasResponse(
    IReadOnlyList<ListarPoliticasResponseItem> Items, long Total);
