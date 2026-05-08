using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.EnviarParaAprovacao;

/// <summary>
/// Behavior específico do EnviarParaAprovacaoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class EnviarParaAprovacaoCommandBehavior
    : IPipelineBehavior<EnviarParaAprovacaoCommand, ResponseDefault<EnviarParaAprovacaoCommandResult>>
{
    public Task<ResponseDefault<EnviarParaAprovacaoCommandResult>> Handle(
        EnviarParaAprovacaoCommand request,
        RequestHandlerDelegate<ResponseDefault<EnviarParaAprovacaoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
