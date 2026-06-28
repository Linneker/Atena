using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CriarPolitica;

public sealed class CriarPoliticaCommandBehavior
    : IPipelineBehavior<CriarPoliticaCommand, ResponseDefault<CriarPoliticaCommandResult>>
{
    public Task<ResponseDefault<CriarPoliticaCommandResult>> Handle(
        CriarPoliticaCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarPoliticaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
