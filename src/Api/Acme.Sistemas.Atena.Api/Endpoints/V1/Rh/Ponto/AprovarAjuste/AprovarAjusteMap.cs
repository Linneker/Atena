using Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.AprovarAjuste;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.AprovarAjuste;

public static class AprovarAjusteMap
{
    public static AprovarAjusteCommand ToCommand(this AprovarAjusteRequest r) => new(r.Id, r.Justificativa);
    public static AprovarAjusteResponse ToResponse(this AprovarAjusteCommandResult r)
        => new(r.AjusteId, r.MarcacaoResultanteId);
}
