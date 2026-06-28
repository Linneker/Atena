using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.AprovarAjuste;

public sealed class AprovarAjusteCommandBehavior
    : IPipelineBehavior<AprovarAjusteCommand, ResponseDefault<AprovarAjusteCommandResult>>
{
    public Task<ResponseDefault<AprovarAjusteCommandResult>> Handle(
        AprovarAjusteCommand request,
        RequestHandlerDelegate<ResponseDefault<AprovarAjusteCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
