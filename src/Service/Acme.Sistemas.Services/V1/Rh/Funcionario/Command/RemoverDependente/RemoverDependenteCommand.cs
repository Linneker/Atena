using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverDependente;

public sealed record RemoverDependenteCommand(Guid DependenteId)
    : IRequest<ResponseDefault<RemoverDependenteCommandResult>>;
