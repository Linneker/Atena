using Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.FecharCompetencia;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.FecharCompetencia;

public static class FecharCompetenciaMap
{
    public static FecharCompetenciaCommand ToCommand(this FecharCompetenciaRequest r)
        => new(r.FuncionarioId, r.Competencia, r.Observacoes);

    public static FecharCompetenciaResponse ToResponse(this FecharCompetenciaCommandResult r)
        => new(r.FechamentoId, r.Competencia);
}
