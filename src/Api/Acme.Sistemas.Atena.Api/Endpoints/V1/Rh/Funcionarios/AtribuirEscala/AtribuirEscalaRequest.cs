namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.AtribuirEscala;

public sealed record AtribuirEscalaRequest(
    Guid FuncionarioId,
    Guid JornadaId,
    DateOnly VigenciaInicio,
    string? Observacao);
