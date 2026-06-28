using Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.RejeitarAjuste;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.RejeitarAjuste;

public static class RejeitarAjusteMap
{
    public static RejeitarAjusteCommand ToCommand(this RejeitarAjusteRequest r) => new(r.Id, r.Justificativa);
    public static RejeitarAjusteResponse ToResponse(this RejeitarAjusteCommandResult r) => new(r.Id);
}
