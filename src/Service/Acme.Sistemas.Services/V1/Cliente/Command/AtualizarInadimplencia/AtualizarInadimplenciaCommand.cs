using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cliente.Command.AtualizarInadimplencia;

public sealed record AtualizarInadimplenciaCommand(
    Guid Id,
    bool Inadimplente,
    bool BloquearVendas) : IRequest<ResponseDefault<AtualizarInadimplenciaCommandResult>>;

