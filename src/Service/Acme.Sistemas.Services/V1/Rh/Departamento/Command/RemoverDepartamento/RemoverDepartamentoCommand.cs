using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.RemoverDepartamento;

public sealed record RemoverDepartamentoCommand(Guid Id)
    : IRequest<ResponseDefault<RemoverDepartamentoCommandResult>>;
