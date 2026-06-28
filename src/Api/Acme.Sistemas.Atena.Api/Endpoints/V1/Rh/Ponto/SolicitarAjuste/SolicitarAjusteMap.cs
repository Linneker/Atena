using Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.SolicitarAjuste;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.SolicitarAjuste;

public static class SolicitarAjusteMap
{
    public static SolicitarAjusteCommand ToCommand(this SolicitarAjusteRequest r)
        => new(r.MarcacaoOriginalId, r.TipoAjuste, r.DataHoraProposta,
               r.TipoMarcacaoProposta, r.Motivo, r.AnexoUrl);

    public static SolicitarAjusteResponse ToResponse(this SolicitarAjusteCommandResult r) => new(r.Id);
}
