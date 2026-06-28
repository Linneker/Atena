using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RegistrarReajusteSalarial;

public sealed class RegistrarReajusteSalarialCommandBehavior
    : IPipelineBehavior<RegistrarReajusteSalarialCommand, ResponseDefault<RegistrarReajusteSalarialCommandResult>>
{
    public Task<ResponseDefault<RegistrarReajusteSalarialCommandResult>> Handle(
        RegistrarReajusteSalarialCommand request,
        RequestHandlerDelegate<ResponseDefault<RegistrarReajusteSalarialCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
