using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CadastrarDependente;

public sealed class CadastrarDependenteCommandBehavior
    : IPipelineBehavior<CadastrarDependenteCommand, ResponseDefault<CadastrarDependenteCommandResult>>
{
    public Task<ResponseDefault<CadastrarDependenteCommandResult>> Handle(
        CadastrarDependenteCommand request,
        RequestHandlerDelegate<ResponseDefault<CadastrarDependenteCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
