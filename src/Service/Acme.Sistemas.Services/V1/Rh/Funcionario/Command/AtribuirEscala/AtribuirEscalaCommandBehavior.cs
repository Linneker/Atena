using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AtribuirEscala;

public sealed class AtribuirEscalaCommandBehavior
    : IPipelineBehavior<AtribuirEscalaCommand, ResponseDefault<AtribuirEscalaCommandResult>>
{
    public Task<ResponseDefault<AtribuirEscalaCommandResult>> Handle(
        AtribuirEscalaCommand request,
        RequestHandlerDelegate<ResponseDefault<AtribuirEscalaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
