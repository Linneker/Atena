using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Empresa.Command.AlterarEmpresa;

/// <summary>
/// Behavior específico do AlterarEmpresaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarEmpresaCommandBehavior
    : IPipelineBehavior<AlterarEmpresaCommand, ResponseDefault<AlterarEmpresaCommandResult>>
{
    public Task<ResponseDefault<AlterarEmpresaCommandResult>> Handle(
        AlterarEmpresaCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarEmpresaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
