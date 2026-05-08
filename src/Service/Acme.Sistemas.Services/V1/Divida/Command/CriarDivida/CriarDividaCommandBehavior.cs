using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Divida.Command.CriarDivida;

/// <summary>
/// Behavior específico do CriarDividaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarDividaCommandBehavior
    : IPipelineBehavior<CriarDividaCommand, ResponseDefault<CriarDividaCommandResult>>
{
    public Task<ResponseDefault<CriarDividaCommandResult>> Handle(
        CriarDividaCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarDividaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
