using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RegistrarReajusteSalarial;

public sealed record RegistrarReajusteSalarialRequest(
    Guid FuncionarioId,
    decimal NovoValor,
    DateOnly VigenciaInicio,
    MotivoSalario Motivo,
    string? Observacao);
