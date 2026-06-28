using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.PagarSaldo;

public sealed class PagarSaldoCommandBehavior
    : IPipelineBehavior<PagarSaldoCommand, ResponseDefault<PagarSaldoCommandResult>>
{
    public Task<ResponseDefault<PagarSaldoCommandResult>> Handle(
        PagarSaldoCommand request,
        RequestHandlerDelegate<ResponseDefault<PagarSaldoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
