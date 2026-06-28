using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using CargoEntity = Acme.Sistemas.Domain.Entities.Rh.Cargo;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.CriarCargo;

public sealed class CriarCargoCommandHandler
    : IRequestHandler<CriarCargoCommand, ResponseDefault<CriarCargoCommandResult>>
{
    private readonly ICargoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarCargoCommandHandler(ICargoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarCargoCommandResult>> Handle(
        CriarCargoCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.Codigo))
        {
            var existente = await _repo.GetByCodigoAsync(request.Codigo, cancellationToken);
            if (existente is not null)
                return ResponseDefault<CriarCargoCommandResult>.Conflict(
                    $"Já existe um cargo com o código '{request.Codigo}'.");
        }

        var cargo = new CargoEntity
        {
            TenantId = _tenantContext.TenantId,
            Codigo = request.Codigo,
            Descricao = request.Descricao,
            CodigoCbo = request.CodigoCbo,
            SalarioBaseSugerido = request.SalarioBaseSugerido,
            Ativo = true,
            CreatedBy = _tenantContext.UserId,
        };

        await _repo.AddAsync(cargo, cancellationToken);

        return ResponseDefault<CriarCargoCommandResult>.Created(
            new CriarCargoCommandResult(cargo.Id, cargo.Descricao));
    }
}
