using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ObterUsuario;

/// <summary>
/// Behavior específico do ObterUsuarioQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterUsuarioQueryBehavior
    : IPipelineBehavior<ObterUsuarioQuery, ResponseDefault<ObterUsuarioQueryResult>>
{
    public Task<ResponseDefault<ObterUsuarioQueryResult>> Handle(
        ObterUsuarioQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterUsuarioQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
