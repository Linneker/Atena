using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AtribuirEscala;

public sealed record AtribuirEscalaCommand(
    Guid FuncionarioId,
    Guid JornadaId,
    DateOnly VigenciaInicio,
    string? Observacao)
    : IRequest<ResponseDefault<AtribuirEscalaCommandResult>>;
