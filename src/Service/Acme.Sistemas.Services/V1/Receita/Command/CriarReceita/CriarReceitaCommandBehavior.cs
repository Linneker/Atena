using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Command.CriarReceita;

/// <summary>
/// Behavior específico do CriarReceitaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarReceitaCommandBehavior
    : IPipelineBehavior<CriarReceitaCommand, ResponseDefault<CriarReceitaCommandResult>>
{
    public Task<ResponseDefault<CriarReceitaCommandResult>> Handle(
        CriarReceitaCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarReceitaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
