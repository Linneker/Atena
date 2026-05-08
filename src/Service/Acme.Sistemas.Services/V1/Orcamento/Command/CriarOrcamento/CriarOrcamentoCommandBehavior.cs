using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Orcamento.Command.CriarOrcamento;

/// <summary>
/// Behavior específico do CriarOrcamentoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarOrcamentoCommandBehavior
    : IPipelineBehavior<CriarOrcamentoCommand, ResponseDefault<CriarOrcamentoCommandResult>>
{
    public Task<ResponseDefault<CriarOrcamentoCommandResult>> Handle(
        CriarOrcamentoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarOrcamentoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
