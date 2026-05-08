using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Divida.Command.AlterarDivida;

/// <summary>
/// Behavior específico do AlterarDividaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarDividaCommandBehavior
    : IPipelineBehavior<AlterarDividaCommand, ResponseDefault<AlterarDividaCommandResult>>
{
    public Task<ResponseDefault<AlterarDividaCommandResult>> Handle(
        AlterarDividaCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarDividaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
