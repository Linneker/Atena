using Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.ReabrirCompetencia;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ReabrirCompetencia;

public static class ReabrirCompetenciaMap
{
    public static ReabrirCompetenciaCommand ToCommand(this ReabrirCompetenciaRequest r)
        => new(r.FuncionarioId, r.Competencia, r.Motivo);

    public static ReabrirCompetenciaResponse ToResponse(this ReabrirCompetenciaCommandResult r)
        => new(r.FechamentoId);
}
