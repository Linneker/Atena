namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ReabrirCompetencia;

public sealed record ReabrirCompetenciaRequest(Guid FuncionarioId, string Competencia, string Motivo);
