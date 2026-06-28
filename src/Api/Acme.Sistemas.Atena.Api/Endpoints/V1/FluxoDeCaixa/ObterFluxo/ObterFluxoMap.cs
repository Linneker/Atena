using Acme.Sistemas.Services.V1.FluxoDeCaixa.Query.ObterFluxo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FluxoDeCaixa.ObterFluxo;

public static class ObterFluxoMap
{
    public static ObterFluxoQuery ToQuery(this ObterFluxoRequest request)
        => new(request.Inicio, request.Fim, request.SomenteRealizados);

    public static ObterFluxoResponse ToResponse(this ObterFluxoQueryResult result)
        => new(
            result.Inicio,
            result.Fim,
            result.TotalReceitas,
            result.TotalDespesas,
            result.Resultado,
            result.SomenteRealizados,
            result.PeriodoFechado,
            result.Movimentos.Select(m => new FluxoMovimentoResponseItem(
                m.Data, m.Tipo, m.Descricao, m.Valor, m.Status, m.Realizado)).ToArray());
}
