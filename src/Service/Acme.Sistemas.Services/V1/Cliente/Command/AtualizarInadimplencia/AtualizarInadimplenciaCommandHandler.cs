using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Cliente.Command.AtualizarInadimplencia;

public sealed class AtualizarInadimplenciaCommandHandler
    : IRequestHandler<AtualizarInadimplenciaCommand, ResponseDefault<AtualizarInadimplenciaCommandResult>>
{
    private readonly IClienteRepository _repo;

    public AtualizarInadimplenciaCommandHandler(IClienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<AtualizarInadimplenciaCommandResult>> Handle(AtualizarInadimplenciaCommand request, CancellationToken cancellationToken)
    {
        var cliente = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (cliente is null)
            return ResponseDefault<AtualizarInadimplenciaCommandResult>.NotFound("Cliente não encontrado.");

        await _repo.UpdateInadimplenciaAsync(cliente.Id, request.Inadimplente, request.BloquearVendas, cancellationToken);

        return ResponseDefault<AtualizarInadimplenciaCommandResult>.Ok(
            new AtualizarInadimplenciaCommandResult(cliente.Id, request.Inadimplente, request.BloquearVendas));
    }
}
