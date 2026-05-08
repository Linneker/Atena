using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Funcionario.Query.ListarFuncionarios;

/// <summary>
/// Behavior específico do ListarFuncionariosQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarFuncionariosQueryBehavior
    : IPipelineBehavior<ListarFuncionariosQuery, ResponseDefault<ListarFuncionariosQueryResult>>
{
    public Task<ResponseDefault<ListarFuncionariosQueryResult>> Handle(
        ListarFuncionariosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarFuncionariosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
