namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarMarcacoesEquipe;

public sealed record ListarMarcacoesEquipeRequest(
    Guid FuncionarioId,
    DateOnly DataInicio,
    DateOnly DataFim);
