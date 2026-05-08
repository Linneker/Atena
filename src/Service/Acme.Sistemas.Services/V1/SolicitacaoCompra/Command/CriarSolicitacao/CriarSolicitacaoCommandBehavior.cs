using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.CriarSolicitacao;

/// <summary>
/// Behavior específico do CriarSolicitacaoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarSolicitacaoCommandBehavior
    : IPipelineBehavior<CriarSolicitacaoCommand, ResponseDefault<CriarSolicitacaoCommandResult>>
{
    public Task<ResponseDefault<CriarSolicitacaoCommandResult>> Handle(
        CriarSolicitacaoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarSolicitacaoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
