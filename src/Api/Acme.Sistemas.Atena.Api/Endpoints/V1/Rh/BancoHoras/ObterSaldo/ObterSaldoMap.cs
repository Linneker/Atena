using Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ObterSaldo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.ObterSaldo;

public static class ObterSaldoMap
{
    public static ObterSaldoQuery ToQuery(this ObterSaldoRequest r) => new(r.FuncionarioId, r.Competencia);

    public static ObterSaldoResponse ToResponse(this ObterSaldoQueryResult r)
        => new(r.FuncionarioId, r.Competencia, r.HorasDevidas, r.HorasRealizadas, r.SaldoMinutos, r.PoliticaId);
}
