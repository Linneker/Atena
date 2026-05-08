using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EnviarDanfe;

/// <summary>
/// Behavior específico do EnviarDanfeCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class EnviarDanfeCommandBehavior
    : IPipelineBehavior<EnviarDanfeCommand, ResponseDefault<EnviarDanfeCommandResult>>
{
    public Task<ResponseDefault<EnviarDanfeCommandResult>> Handle(
        EnviarDanfeCommand request,
        RequestHandlerDelegate<ResponseDefault<EnviarDanfeCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
