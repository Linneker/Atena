using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.AlterarCargo;

public sealed class AlterarCargoCommandHandler
    : IRequestHandler<AlterarCargoCommand, ResponseDefault<AlterarCargoCommandResult>>
{
    private readonly ICargoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarCargoCommandHandler(ICargoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarCargoCommandResult>> Handle(
        AlterarCargoCommand request, CancellationToken cancellationToken)
    {
        var cargo = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (cargo is null)
            return ResponseDefault<AlterarCargoCommandResult>.NotFound(
                $"Cargo {request.Id} não encontrado.");

        if (!string.IsNullOrWhiteSpace(request.Codigo) &&
            !string.Equals(cargo.Codigo, request.Codigo, StringComparison.OrdinalIgnoreCase))
        {
            var conflito = await _repo.GetByCodigoAsync(request.Codigo, cancellationToken);
            if (conflito is not null && conflito.Id != request.Id)
                return ResponseDefault<AlterarCargoCommandResult>.Conflict(
                    $"Já existe um cargo com o código '{request.Codigo}'.");
        }

        cargo.Codigo = request.Codigo;
        cargo.Descricao = request.Descricao;
        cargo.CodigoCbo = request.CodigoCbo;
        cargo.SalarioBaseSugerido = request.SalarioBaseSugerido;
        cargo.Ativo = request.Ativo;
        cargo.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(cargo, cancellationToken);

        return ResponseDefault<AlterarCargoCommandResult>.Ok(
            new AlterarCargoCommandResult(cargo.Id));
    }
}
