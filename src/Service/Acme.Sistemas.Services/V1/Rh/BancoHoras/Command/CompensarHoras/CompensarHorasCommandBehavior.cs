using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CompensarHoras;

public sealed class CompensarHorasCommandBehavior
    : IPipelineBehavior<CompensarHorasCommand, ResponseDefault<CompensarHorasCommandResult>>
{
    public Task<ResponseDefault<CompensarHorasCommandResult>> Handle(
        CompensarHorasCommand request,
        RequestHandlerDelegate<ResponseDefault<CompensarHorasCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
