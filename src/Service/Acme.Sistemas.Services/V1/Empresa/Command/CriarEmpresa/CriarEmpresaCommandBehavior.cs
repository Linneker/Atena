using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

/// <summary>
/// Behavior específico do CriarEmpresaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarEmpresaCommandBehavior
    : IPipelineBehavior<CriarEmpresaCommand, ResponseDefault<CriarEmpresaCommandResult>>
{
    public Task<ResponseDefault<CriarEmpresaCommandResult>> Handle(
        CriarEmpresaCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarEmpresaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
