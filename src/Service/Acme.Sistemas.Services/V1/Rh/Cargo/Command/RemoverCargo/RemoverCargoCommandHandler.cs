using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.RemoverCargo;

public sealed class RemoverCargoCommandHandler
    : IRequestHandler<RemoverCargoCommand, ResponseDefault<RemoverCargoCommandResult>>
{
    private readonly ICargoRepository _repo;

    public RemoverCargoCommandHandler(ICargoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<RemoverCargoCommandResult>> Handle(
        RemoverCargoCommand request, CancellationToken cancellationToken)
    {
        var cargo = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (cargo is null)
            return ResponseDefault<RemoverCargoCommandResult>.NotFound(
                $"Cargo {request.Id} não encontrado.");

        // Soft delete: funcionarios.cargo_id ainda referencia; preserva histórico.
        await _repo.DeleteAsync(request.Id, cancellationToken);

        return ResponseDefault<RemoverCargoCommandResult>.Ok(
            new RemoverCargoCommandResult(request.Id));
    }
}
