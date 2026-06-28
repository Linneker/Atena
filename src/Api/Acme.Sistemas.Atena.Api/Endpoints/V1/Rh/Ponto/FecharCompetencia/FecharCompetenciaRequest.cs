namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.FecharCompetencia;

public sealed record FecharCompetenciaRequest(Guid FuncionarioId, string Competencia, string? Observacoes);
