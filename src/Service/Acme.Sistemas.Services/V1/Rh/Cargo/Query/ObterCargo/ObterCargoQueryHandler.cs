using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ObterCargo;

public sealed class ObterCargoQueryHandler
    : IRequestHandler<ObterCargoQuery, ResponseDefault<ObterCargoQueryResult>>
{
    private readonly ICargoRepository _repo;

    public ObterCargoQueryHandler(ICargoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ObterCargoQueryResult>> Handle(
        ObterCargoQuery request, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (c is null)
            return ResponseDefault<ObterCargoQueryResult>.NotFound($"Cargo {request.Id} não encontrado.");

        return ResponseDefault<ObterCargoQueryResult>.Ok(new ObterCargoQueryResult(
            c.Id, c.Codigo, c.Descricao, c.CodigoCbo, c.SalarioBaseSugerido, c.Ativo));
    }
}
