using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverDependente;

public sealed class RemoverDependenteCommandHandler
    : IRequestHandler<RemoverDependenteCommand, ResponseDefault<RemoverDependenteCommandResult>>
{
    private readonly IDependenteRepository _repo;

    public RemoverDependenteCommandHandler(IDependenteRepository repo) => _repo = repo;

    public async Task<ResponseDefault<RemoverDependenteCommandResult>> Handle(
        RemoverDependenteCommand request, CancellationToken cancellationToken)
    {
        var dep = await _repo.GetByIdAsync(request.DependenteId, cancellationToken);
        if (dep is null)
            return ResponseDefault<RemoverDependenteCommandResult>.NotFound(
                $"Dependente {request.DependenteId} não encontrado.");

        await _repo.DeleteAsync(request.DependenteId, cancellationToken);

        return ResponseDefault<RemoverDependenteCommandResult>.Ok(
            new RemoverDependenteCommandResult(request.DependenteId));
    }
}
