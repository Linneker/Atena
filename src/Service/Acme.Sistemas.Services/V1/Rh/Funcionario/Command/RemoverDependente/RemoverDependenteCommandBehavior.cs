using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverDependente;

public sealed class RemoverDependenteCommandBehavior
    : IPipelineBehavior<RemoverDependenteCommand, ResponseDefault<RemoverDependenteCommandResult>>
{
    public Task<ResponseDefault<RemoverDependenteCommandResult>> Handle(
        RemoverDependenteCommand request,
        RequestHandlerDelegate<ResponseDefault<RemoverDependenteCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
