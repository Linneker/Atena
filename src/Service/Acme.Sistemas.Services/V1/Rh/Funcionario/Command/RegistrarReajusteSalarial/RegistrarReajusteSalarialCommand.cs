using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RegistrarReajusteSalarial;

public sealed record RegistrarReajusteSalarialCommand(
    Guid FuncionarioId,
    decimal NovoValor,
    DateOnly VigenciaInicio,
    MotivoSalario Motivo,
    string? Observacao)
    : IRequest<ResponseDefault<RegistrarReajusteSalarialCommandResult>>;
