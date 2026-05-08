using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ListarSolicitacoes;

/// <summary>
/// Behavior específico do ListarSolicitacoesQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarSolicitacoesQueryBehavior
    : IPipelineBehavior<ListarSolicitacoesQuery, ResponseDefault<ListarSolicitacoesQueryResult>>
{
    public Task<ResponseDefault<ListarSolicitacoesQueryResult>> Handle(
        ListarSolicitacoesQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarSolicitacoesQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
