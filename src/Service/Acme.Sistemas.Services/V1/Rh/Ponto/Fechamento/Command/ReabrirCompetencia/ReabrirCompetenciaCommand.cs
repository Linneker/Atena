using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.ReabrirCompetencia;

public sealed record ReabrirCompetenciaCommand(Guid FuncionarioId, string Competencia, string Motivo)
    : IRequest<ResponseDefault<ReabrirCompetenciaCommandResult>>;
