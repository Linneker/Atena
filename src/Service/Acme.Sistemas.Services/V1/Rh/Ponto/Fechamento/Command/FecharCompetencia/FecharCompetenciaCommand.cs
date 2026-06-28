using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.FecharCompetencia;

public sealed record FecharCompetenciaCommand(Guid FuncionarioId, string Competencia, string? Observacoes)
    : IRequest<ResponseDefault<FecharCompetenciaCommandResult>>;
