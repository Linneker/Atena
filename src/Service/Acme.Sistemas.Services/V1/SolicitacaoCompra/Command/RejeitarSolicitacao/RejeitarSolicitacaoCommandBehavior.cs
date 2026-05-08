using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.RejeitarSolicitacao;

/// <summary>
/// Behavior específico do RejeitarSolicitacaoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class RejeitarSolicitacaoCommandBehavior
    : IPipelineBehavior<RejeitarSolicitacaoCommand, ResponseDefault<RejeitarSolicitacaoCommandResult>>
{
    public Task<ResponseDefault<RejeitarSolicitacaoCommandResult>> Handle(
        RejeitarSolicitacaoCommand request,
        RequestHandlerDelegate<ResponseDefault<RejeitarSolicitacaoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
