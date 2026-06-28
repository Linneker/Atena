namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FluxoDeCaixa.ObterFluxo;

public sealed record ObterFluxoRequest(
    DateTime Inicio,
    DateTime Fim,
    bool SomenteRealizados = false);
